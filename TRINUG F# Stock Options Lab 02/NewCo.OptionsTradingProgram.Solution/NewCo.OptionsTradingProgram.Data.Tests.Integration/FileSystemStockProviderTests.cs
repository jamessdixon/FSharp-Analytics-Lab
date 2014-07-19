using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewCo.OptionsTradingProgram.Data.Tests.Integration
{
    [TestClass]
    public class FileSystemStockProviderTests
    {
        [TestMethod]
        public void PutData_ReturnsExpected()
        {
            FileSystemStockProvider provider = new FileSystemStockProvider(@"C:\Data\TEST.txt");
            var data = provider.GetData();
            if (data != null)
            {
                provider.PutData(data);
            }
        }

        [TestMethod]
        public void GetData_ReturnsExpected()
        {
            FileSystemStockProvider provider = new FileSystemStockProvider(@"C:\Data\TEST.txt");
            var data = provider.GetData();
            Assert.IsNotNull(data);
        }
    }
}
