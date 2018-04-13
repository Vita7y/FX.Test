using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FX.Test.Core
{
    public class TradeLoaderXml: ITradeLoader
    {
        public IEnumerable<ITrade> GetTrades(StreamReader stream)
        {
            var str = stream.ReadToEnd();
            var res = str.DeserializeFromString<Portfolio>();
            return res.Trade;
        }

        [Serializable]
        [System.ComponentModel.DesignerCategory("code")]
        [XmlType(AnonymousType = true)]
        [XmlRoot("portfolio", IsNullable = false)]
        public class Portfolio
        {
            private Trade[] _tradeField;

            /// <remarks/>
            [XmlElement("trade")]
            public Trade[] Trade
            {
                get
                {
                    return _tradeField;
                }
                set
                {
                    _tradeField = value;
                }
            }
        }
    }
}