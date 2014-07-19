using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewCo.TwitterAnalysis.Tests
{
    [TestClass]
    public class AnalysisEngineTests
    {
        [TestMethod]
        public void CalculateCorrelationUsingValidValues_returnsExpected()
        {
            ITweetProvider tweetProvider = new InMemoryTweetProvider();
            IStockProvider stockProvider = new InMemoryStockProvider();

            AnalysisEngine engine = new AnalysisEngine(tweetProvider, stockProvider);

            float expected = 1.0F;
            var actual = engine.CalculateCorrelation("TEST");

            Assert.AreEqual(expected, actual);
        }
    }
}
