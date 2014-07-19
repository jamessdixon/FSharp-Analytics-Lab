using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace NewCo.TwitterAnalysis.Tests
{
    [TestClass]
    public class InMemoryStockProviderTests
    {
        [TestMethod]
        public void GetDailyVolumeOfTradesValidInput_ReturnsExpectedValue()
        {
            IStockProvider provider = new InMemoryStockProvider();
            var dailyTotal = provider.GetDailyVolumeOfTrades("Test");
            var tweetList = dailyTotal.ToList();
            Int32 expected = 2;
            Int32 actual = tweetList.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
