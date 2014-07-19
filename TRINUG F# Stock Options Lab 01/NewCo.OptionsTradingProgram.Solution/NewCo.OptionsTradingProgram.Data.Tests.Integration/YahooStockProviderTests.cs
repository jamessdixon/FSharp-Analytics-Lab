using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewCo.OptionsTradingProgram.Data.Tests.Integration
{
    [TestClass]
    public class YahooStockProviderTests
    {
        [TestMethod]
        public void YahooStockProviderUsingMSFT_ReturnsExpected()
        {
            YahooStockProvider stockProvider = new YahooStockProvider();
            var data = stockProvider.GetData("MSFT");
            Assert.IsNotNull(data);
        }
    }
}
