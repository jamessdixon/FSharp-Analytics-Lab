using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewCo.TwitterAnalysis.Tests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var provider = new TestTweetProvider();
            var tweets = provider.GetTweets("TRINUG");
            Assert.AreEqual(1, 1);
        }
    }
}
