using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCo.OptionsTradingProgram.Analytics;
using System.Collections.Generic;

namespace NewCo.OptionsTradingProgram.Analytics.Tests.Unit
{
    [TestClass]
    public class CalculationsTests
    {
        [TestMethod]
        public void CalculateVarianceUsingValidData_ReturnsExpected()
        {
            var testData = new Double[6] { 1, 2, 3, 4, 5, 6 };
            var calculations = new Calculations();
            var variance = calculations.Variance(testData);

            var expected = 2.916666667;
            var actual = Math.Round(variance, 9);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalcualteStandardDeviationUsingValidData_ReturnsExpected()
        {
            var testData = new Double[6] { 1, 2, 3, 4, 5, 6 };
            var calculations = new Calculations();
            var standadDeviation = calculations.StandardDeviation(testData);

            var expected = 1.707825128;
            var actual =  Math.Round(standadDeviation,9);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMovingAverageUsingValidData_ReturnsExpected()
        {
            var testData = new Double[6] { 1, 2, 3, 4, 5, 6 };
            var calculations = new Calculations();
            var movingAverage = calculations.MovingAverage(testData,3);

            var expected = 4;
            var actual = new List<Double>(movingAverage).Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateMovingStandardDeviationUsingValidData_ReturnsExpected()
        {
            var testData = new Double[6] { 1, 2, 3, 4, 5, 6 };
            var calculations = new Calculations();
            var movingStandardDeviation = calculations.MovingStandardDeviation(testData,3);

            var expected = 4;
            var actual = new List<Double>(movingStandardDeviation).Count;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void CalculateBollingerBandsUsingValidData_ReturnsExpected()
        {
            var testData = new Double[6] { 1, 2, 3, 4, 5, 6 };
            var calculations = new Calculations();
            var bollingerBands = calculations.BollingerBands(testData,3);

            var expected = 4;
            var actual = new List<Tuple<Double,Double>>(bollingerBands).Count;
            Assert.AreEqual(expected, actual);

        }
    }
}
