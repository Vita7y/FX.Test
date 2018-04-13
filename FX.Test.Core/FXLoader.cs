using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace FX.Test.Core
{
    public class FXLoader
    {
        public IEnumerable<FXOption> GetTrades(StreamReader stream)
        {
            var csv = new CsvReader(stream, new CsvConfiguration() { Delimiter = ",", HasHeaderRecord = true });
            csv.Configuration.RegisterClassMap<TradeClassMap>();
            var res = csv.GetRecords<FXOption>();
            return res.ToList();
        }

        public sealed class TradeClassMap : CsvClassMap<FXOption>
        {
            public TradeClassMap()
            {
                Map(m => m.Id).Name("Id").Index(0);
                Map(m => m.CCYPair).Name("CCY pair").Index(1);
                Map(m => m.Type).Name("Type").Index(2);
                Map(m => m.Style).Name("Style").Index(3);
                Map(m => m.CP).Name("C/P").Index(4);
                Map(m => m.Expiry).Name("Expiry").Index(5);
                Map(m => m.StrikePrice).Name("Strike rate").Index(6);
                Map(m => m.Notional).Name("Notional amount").Index(7);
                Map(m => m.CCY).Name("CCY").Index(8);
            }
        }
    }
}