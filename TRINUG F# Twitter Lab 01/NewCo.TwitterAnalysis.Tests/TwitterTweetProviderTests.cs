using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewCo.TwitterAnalysis.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetTweetsUsingIBM_returnsExpectedValue()
        {
            ITweetProvider provider = new TwitterTweetProvider();
            var actual = provider.GetTweets("TRINUG");
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GetDailyTotalUsingIBM_returnsExpectedValue()
        {
            ITweetProvider provider = new TwitterTweetProvider();
            var actual = provider.GetDailyTotalOfTweets("IBM");
            Assert.IsNotNull(actual);
        }
    }
}
