using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using FX.Test.Core.Properties;

namespace FX.Test.Core
{
    public class Manager: INotifyPropertyChanged
    {
        public enum FileType
        {
            XML,
            CSV
        }

        private readonly Dictionary<FileType, ITradeLoader> _loaders;

        public Manager()
        {
            Parameters = new Dictionary<string, CalcOptionParameters>
            {
                {"ABC INC", new CalcOptionParameters(DateTime.Parse("2016.04.01"), 30, 0.40, 0.05, 0)},
                {"CDE LTD", new CalcOptionParameters(DateTime.Parse("2016.04.01"), 110, 0.05, 0.03, 0)}
            };

            _loaders = new Dictionary<FileType, ITradeLoader>
            {
                {FileType.CSV, new TradeLoaderCsv()},
                { FileType.XML, new TradeLoaderXml()}
            };

            Data =new BindingList<EuropianTradeOption>();
            CurrencyPair = new Dictionary<string, double>()
            {
                {"USDCHF", 0.963286 },
                {"PLNCHF", 0.267377 },
                {"USDZAR", 13.1563 },
                {"PLNZAR", 3.65292 }
            };

            Volatility = new List<double>(){ 10, 5, 2, 1};
            SpotPrice = new List<double>(){ 100, 50, 25, 10};
        }

        private string _total;
        public string TotalPorfolio
        {
            get { return _total; }
            private set
            {
                _total = value;
                OnPropertyChanged(nameof(TotalPorfolio));
            }
        }

        public BindingList<EuropianTradeOption> Data { get; }

        public Dictionary<string, CalcOptionParameters> Parameters { get; }

        public Dictionary<string, double> CurrencyPair { get; }

        public List<double> Volatility { get; }

        public List<double> SpotPrice { get; }

        public void LoadAndSetData(string fileName)
        {
            Data.Clear();
            var data = LoadData(fileName);
            var res = ConverToOptions(data);
            foreach (var trade in res)
            {
                Data.Add(trade);
            }

            var ccyGroup = Data.GroupBy(am => am.CCY);
            TotalPorfolio = "Totals:";
            foreach (var options in ccyGroup)
                TotalPorfolio += $"CCY ={options.Key}| Put = {options.Sum(am => am.Put):f2}| Sell = {options.Sum(pm => pm.Call):f2}|| ";
        }

        private IEnumerable<ITrade> LoadData(string fileName)
        {
            var strExt = Path.GetExtension(fileName).Replace(".", "").ToUpper();
            var ext = (FileType) Enum.Parse(typeof(FileType), strExt);
            return LoadData(ext, fileName);
        }

        private IEnumerable<ITrade> LoadData(FileType fileType, string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName");
            if(!File.Exists(fileName))
                throw new FileNotFoundException($"File: {fileName} not found.");
            if(!_loaders.ContainsKey(fileType))
                throw new EntryPointNotFoundException($"Converter to {fileType} not found.");

            return _loaders[fileType].GetTrades(File.OpenText(fileName));
        }

        private IEnumerable<EuropianTradeOption> ConverToOptions(IEnumerable<ITrade> trades)
        {
            return trades.Select(trade => new EuropianTradeOption(trade, Parameters[trade.Name], TradeHelper.GetPut, TradeHelper.GetCall));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //It`s very simple realization of matrix of values, and I don`t like this realization
        public void CreateMatrix()
        {
            var list = new List<EuropianTradeOption>(Data);
            Data.Clear();
            foreach (var option in list)
            {
                var param = Parameters[option.Name];
                Data.Add(new EuropianTradeOption(option, param, TradeHelper.GetPut, TradeHelper.GetCall));

                foreach (var vol in Volatility)
                {
                    var paramVol = param;
                    paramVol.Volatility = param.Volatility + Math.Round(param.Volatility / 100 * vol, 2);
                    Data.Add(new EuropianTradeOption(option, paramVol, TradeHelper.GetPut, TradeHelper.GetCall));
                    foreach (var spot in SpotPrice)
                    {
                        paramVol.CurrentSpotPrice = param.CurrentSpotPrice + Math.Round((param.CurrentSpotPrice / 100 * spot), 2);
                        Data.Add(new EuropianTradeOption(option, paramVol, TradeHelper.GetPut, TradeHelper.GetCall));
                        paramVol.CurrentSpotPrice = param.CurrentSpotPrice - Math.Round(param.CurrentSpotPrice / 100 * spot, 2);
                        Data.Add(new EuropianTradeOption(option, paramVol, TradeHelper.GetPut, TradeHelper.GetCall));
                    }

                    paramVol = param;
                    paramVol.Volatility = param.Volatility - Math.Round(param.Volatility / 100 * vol, 2);
                    Data.Add(new EuropianTradeOption(option, paramVol, TradeHelper.GetPut, TradeHelper.GetCall));
                    foreach (var spot in SpotPrice)
                    {
                        paramVol.CurrentSpotPrice = param.CurrentSpotPrice + Math.Round(param.CurrentSpotPrice / 100 * spot, 2);
                        Data.Add(new EuropianTradeOption(option, paramVol, TradeHelper.GetPut, TradeHelper.GetCall));
                        paramVol.CurrentSpotPrice = param.CurrentSpotPrice - Math.Round(param.CurrentSpotPrice / 100 * spot, 2);
                        Data.Add(new EuropianTradeOption(option, paramVol, TradeHelper.GetPut, TradeHelper.GetCall));
                    }
                }
            }
        }
    }
}