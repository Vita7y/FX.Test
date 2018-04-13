using System;
using System.IO;
using System.Linq;
using System.Text;
using FX.Test.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestParse
    {
        [TestMethod]
        public void TestMethod_ParseCsv()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(Properties.Resources.CsvPrice);
            MemoryStream stream = new MemoryStream(byteArray);

            var loader = new TradeLoaderCsv();
            var res = loader.GetTrades(new StreamReader(stream));

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Any());
            Assert.AreEqual(123, res.First().Id);
            Assert.AreEqual("CDE LTD", res.First().Name);
            Assert.AreEqual(100, res.First().StrikePrice);
            Assert.AreEqual(Currency.USD, res.First().CCY);
            Assert.AreEqual(CP.C, res.First().CP);
        }

        [TestMethod]
        public void TestMethod_ParseXml()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(Properties.Resources.XmlPrice);
            MemoryStream stream = new MemoryStream(byteArray);

            var loader = new TradeLoaderXml();
            var res = loader.GetTrades(new StreamReader(stream));

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Any());
            Assert.AreEqual(123, res.First().Id);
            Assert.AreEqual("CDE LTD", res.First().Name);
            Assert.AreEqual(100, res.First().StrikePrice);
            Assert.AreEqual(Currency.USD, res.First().CCY);
            Assert.AreEqual(CP.C, res.First().CP);
        }

        [TestMethod]
        public void TestMethod_ParseFX()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(Properties.Resources.FX);
            MemoryStream stream = new MemoryStream(byteArray);

            var loader = new FXLoader();
            var res = loader.GetTrades(new StreamReader(stream));

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Any());
            Assert.AreEqual(234, res.First().Id);
            Assert.AreEqual("USDPLN", res.First().CCYPair);
            Assert.AreEqual(4, res.First().StrikePrice);
            Assert.AreEqual(Currency.USD, res.First().CCY);
            Assert.AreEqual(CP.C, res.First().CP);
        }

    }
}
