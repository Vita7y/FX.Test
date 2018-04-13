using System;

namespace FX.Test.Core
{
    public class FXOption
    {
        public int Id { get; set; }

        public string CCYPair { get; set; }

        public string Type { get; set; }

        public string Style { get; set; }

        public CP CP { get; set; }

        public DateTime Expiry { get; set; }

        public double StrikePrice { get; set; }

        public double Notional { get; set; }

        public double Amount { get; set; }

        public Currency CCY { get; set; }
    }
}
