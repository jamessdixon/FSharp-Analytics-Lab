using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;
using System.Reflection;


namespace NewCo.TwitterAnalysis.Tests
{
    [TestClass]
    public class FileSystemTweetProviderTests
    {
        [TestMethod]
        public void GetTweetsUsingValidInput_ReturnsExpectedValue()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testFile = Path.Combine(baseDir, "TweetData.csv");
            ITweetProvider provider = new FileSystemTweetProvider(testFile);
            var tweets = provider.GetTweets("TEST");
            var tweetList = tweets.ToList();
            Int32 expected = 2;
            Int32 actual = tweetList.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDailyTotalUsingValidInput_ReturnsExpectedValue()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var testFile = Path.Combine(baseDir, "TweetData.csv");
            ITweetProvider provider = new FileSystemTweetProvider(testFile);
            var tweets = provider.GetDailyTotalOfTweets("TEST");
            var tweetList = tweets.ToList();
            Int32 expected = 1;
            Int32 actual = tweetList.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
