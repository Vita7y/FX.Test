using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace FX.Test.Core
{
    public class TradeLoaderCsv: ITradeLoader
    {
        public IEnumerable<ITrade> GetTrades(StreamReader stream)
        {
            var csv = new CsvReader(stream, new CsvConfiguration(){Delimiter = ",", HasHeaderRecord = true});
            csv.Configuration.RegisterClassMap<TradeClassMap>();
            var res = csv.GetRecords<Trade>();
            return res.ToList();
        }

        public sealed class TradeClassMap : CsvClassMap<Trade>
        {
            public TradeClassMap()
            {
                Map(m => m.Id).Name("Id");
                Map(m => m.Name).Name("Name");
                Map(m => m.Type).Name("Type");
                Map(m => m.Style).Name("Style");
                Map(m => m.CP).Name("C/P");
                Map(m => m.Expiry).Name("Expiry");
                Map(m => m.StrikePrice).Name("Strike price");
                Map(m => m.CCY).Name("CCY");
            }
        }
    }
}