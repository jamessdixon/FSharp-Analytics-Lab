using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewCo.TwitterAnalysis.Tests
{
    [TestClass]
    public class YahooStockProviderTests
    {
        [TestMethod]
        public void GetDailyVolumeOfTradesUsingIBM_returnsExpectedValue()
        {
            IStockProvider provider = new YahooStockProvider();
            var actual = provider.GetDailyVolumeOfTrades("IBM");
            Assert.IsNotNull(actual);
        }
    }
}
