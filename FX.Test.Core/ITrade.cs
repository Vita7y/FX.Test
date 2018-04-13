using System;

namespace FX.Test.Core
{
    public interface ITrade
    {
        int Id { get; }
        string Name { get; }
        string Type { get; }
        string Style { get; }
        CP CP { get; }
        DateTime Expiry { get; }
        double StrikePrice { get; }
        Currency CCY { get; }
    }

    public enum CP
    {
        C,
        P
    }

    public enum Currency
    {
        USD,
        PLN,
        CHF,
        ZAR
    }
}