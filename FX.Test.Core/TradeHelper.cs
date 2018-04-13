using System;

namespace FX.Test.Core
{
    public static class TradeHelper
    {
        public static double GetPut(ITrade trade, CalcOptionParameters parameters)
        {
            var time = (trade.Expiry.Date.ToOADate() - parameters.CurrentDate.Date.ToOADate()) / 365;
            var put = PutOption(parameters.CurrentSpotPrice, trade.StrikePrice, time, parameters.CurrentRisk, parameters.Volatility, parameters.Divident);
            return put;
        }

        public static double GetCall(ITrade trade, CalcOptionParameters parameters)
        {
            var time = (trade.Expiry.Date.ToOADate() - parameters.CurrentDate.Date.ToOADate()) / 365;
            var call = CallOption(parameters.CurrentSpotPrice, trade.StrikePrice, time, parameters.CurrentRisk, parameters.Volatility, parameters.Divident);
            return call;
        }

        private static double DOne(double underlyingPrice, double exercisePrice, double time, double interest, double volatility, double dividend)
        {
            var dOne = (Math.Log(underlyingPrice / exercisePrice) + (interest - dividend + 0.5 * Math.Pow(volatility, 2)) * time) / (volatility * Math.Sqrt(time));
            return dOne;
        }

        private static double NdOne(double underlyingPrice, double exercisePrice, double time, double interest, double volatility, double dividend)
        {
            var dOne = DOne(underlyingPrice, exercisePrice, time, interest, volatility, dividend);
            var ndOne = Math.Exp(-(Math.Pow(dOne, 2) / 2)) / Math.Sqrt(2 * 3.14159265358979);
            return ndOne;
        }

        private static double DTwo(double underlyingPrice, double exercisePrice, double time, double interest, double volatility, double dividend)
        {
            var dTwo = DOne(underlyingPrice, exercisePrice, time, interest, volatility, dividend) - volatility * Math.Sqrt(time);

            return dTwo;
        }

        private static double NdTwo(double underlyingPrice, double exercisePrice, double time, double interest, double volatility, double dividend)
        {
            var ndTwo = NormSDist(DTwo(underlyingPrice, exercisePrice, time, interest, volatility, dividend));

            return ndTwo;
        }

        private static double CallOption(double underlyingPrice, double exercisePrice, double time, double interest, double volatility, double dividend)
        {
            var callOption = Math.Exp(-dividend * time) * underlyingPrice * NormSDist(DOne(underlyingPrice, exercisePrice, time, interest, volatility, dividend)) -
                             exercisePrice * Math.Exp(-interest * time) *
                             NormSDist(DOne(underlyingPrice, exercisePrice, time, interest, volatility, dividend) - volatility * Math.Sqrt(time));

            return callOption;
        }

        private static double PutOption(double underlyingPrice, double exercisePrice, double time, double interest, double volatility, double dividend)
        {
            var putOption =
                exercisePrice * Math.Exp(-interest * time) * NormSDist(-DTwo(underlyingPrice, exercisePrice, time, interest, volatility, dividend)) -
                Math.Exp(-dividend * time) * underlyingPrice * NormSDist(-DOne(underlyingPrice, exercisePrice, time, interest, volatility, dividend));

            return putOption;
        }

        //This is a conversion of the NORMSDIST() function from Excel.  The x value represents the z value and the function
        //converts it into a percentage.  3 standard deviations is 99.86% so NORMSDIST(3) returns .9986...
        public static double NormSDist(double x)
        {
            const double b1 = 0.319381530;
            const double b2 = -0.356563782;
            const double b3 = 1.781477937;
            const double b4 = -1.821255978;
            const double b5 = 1.330274429;
            const double p = 0.2316419;
            const double c = 0.39894228;

            if (x >= 0.0)
            {
                var t = 1.0 / (1.0 + p * x);
                return 1.0 - c * Math.Exp(-x * x / 2.0) * t * (t * (t * (t * (t * b5 + b4) + b3) + b2) + b1);
            }
            else
            {
                var t = 1.0 / (1.0 - p * x);
                return c * Math.Exp(-x * x / 2.0) * t * (t * (t * (t * (t * b5 + b4) + b3) + b2) + b1);
            }
        }
    }
}