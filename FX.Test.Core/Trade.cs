using System;
using System.Xml.Serialization;

namespace FX.Test.Core
{
    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class Trade : ITrade
    {
        protected void Init(ITrade trade)
        {
            Id = trade.Id;
            Name = trade.Name;
            Type = trade.Type;
            Style = trade.Style;
            CP = trade.CP;
            Expiry = trade.Expiry;
            StrikePrice = trade.StrikePrice;
            CCY = trade.CCY;
        }

        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("style")]
        public string Style { get; set; }
        [XmlAttribute("cp")]
        public CP CP { get; set; }
        [XmlAttribute("expiry")]
        public DateTime Expiry { get; set; }
        [XmlAttribute("strike")]
        public double StrikePrice { get; set; }
        [XmlAttribute("ccy")]
        public Currency CCY { get; set; }
    }
}