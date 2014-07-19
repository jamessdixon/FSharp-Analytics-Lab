using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace NewCo.TwitterAnalysis.Tests
{
    [TestClass]
    public class InMemoryTweetProviderTests
    {
        [TestMethod]
        public void GetTweetsUsingValidInput_ReturnsExpectedValue()
        {
            ITweetProvider provider = new InMemoryTweetProvider();
            var tweets = provider.GetTweets("TEST");
            var tweetList = tweets.ToList();
            Int32 expected = 2;
            Int32 actual = tweetList.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDailyTotalOfTweetsUsingValidInput_ReturnsExpectedValue()
        {
            ITweetProvider provider = new InMemoryTweetProvider();
            var dailyTotal = provider.GetDailyTotalOfTweets("Test");
            var tweetList = dailyTotal.ToList();
            Int32 expected = 2;
            Int32 actual = tweetList.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
