using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;
using System.Reflection;


namespace NewCo.TwitterAnalysis.Tests
{
    [TestClass]
    public class FileSystemStockProviderTests
    {
        [TestMethod]
        public void GetDailyVolumeOfTradesUsingValidInput_ReturnsExpectedValue()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testFile = Path.Combine(baseDir, "StockData.csv");
            IStockProvider provider = new FileSystemStockProvider(testFile);
            var tweets = provider.GetDailyVolumeOfTrades("TEST");
            var tweetList = tweets.ToList();
            Int32 expected = 2;
            Int32 actual = tweetList.Count;
            Assert.AreEqual(expected, actual);
        }


    }
}
