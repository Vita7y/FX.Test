using System;
using System.ComponentModel;

namespace FX.Test.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct CalcOptionParameters
    {
        public CalcOptionParameters(DateTime date, double spot, double volatility, double risk, double divident)
        {
            CurrentDate = date;
            CurrentSpotPrice = spot;
            Volatility = volatility;
            CurrentRisk = risk;
            Divident = divident;
        }

        public  DateTime CurrentDate { get; }

        public double CurrentSpotPrice { get; set; }

        public double Volatility { get; set; }

        public double CurrentRisk { get; }

        public double Divident { get; }

        public override string ToString()
        {
            return $"{nameof(CurrentDate)}: {CurrentDate}, {nameof(CurrentSpotPrice)}: {CurrentSpotPrice}, {nameof(Volatility)}: {Volatility}, {nameof(CurrentRisk)}: {CurrentRisk}, {nameof(Divident)}: {Divident}";
        }
    }
}