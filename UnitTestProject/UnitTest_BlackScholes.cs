using System;
using FX.Test.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest_BlackScholes
    {
        [TestMethod]
        public void TestMethod_Put()
        {
            var trade = new Trade()
            {
                Id = 123,
                Name = "CDE LTD",
                Type = "StockOption",
                Style = "European",
                CP = CP.C,
                Expiry = DateTime.Parse("2017-04-01"),
                StrikePrice = 100,
                CCY = Currency.USD
            };
            var param = new CalcOptionParameters(DateTime.Parse("2016.04.01"), 110, 0.05, 0.03, 0);
            var res = TradeHelper.GetPut(trade, param);
            Assert.AreEqual(0.0102, Math.Round(res, 4));
        }

        [TestMethod]
        public void TestMethod_Call()
        {
            var trade = new Trade()
            {
                Id = 123,
                Name = "CDE LTD",
                Type = "StockOption",
                Style = "European",
                CP = CP.C,
                Expiry = DateTime.Parse("2017-04-01"),
                StrikePrice = 100,
                CCY = Currency.USD
            };
            var param = new CalcOptionParameters(DateTime.Parse("2016.04.01"), 110, 0.05, 0.03, 0);
            var res = TradeHelper.GetCall(trade, param);
            Assert.AreEqual(12.9656, Math.Round(res, 4));
        }

    }
}