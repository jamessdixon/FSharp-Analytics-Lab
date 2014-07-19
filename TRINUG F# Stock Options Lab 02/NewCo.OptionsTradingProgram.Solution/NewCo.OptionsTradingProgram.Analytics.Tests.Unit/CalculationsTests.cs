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

        [TestMethod]
        public void PowerUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = 8;
            Double actual = Math.Round(calculations.Power(2.0, 3.0), 0);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CumulativeDistributionUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = .84134;
            Double actual = Math.Round(calculations.CumulativeDistribution(1.0),5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesCallUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = 4.4652;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholes(inputData,PutCallFlag.Call), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesPutUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = 5.56595;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholes(inputData, PutCallFlag.Put), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DaysToYearsUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = .08214;
            Double actual = Math.Round(calculations.DaysToYears(30), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesDeltaCallUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = .50732;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholesDelta(inputData, PutCallFlag.Call), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesDeltaPutUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = -.49268;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholesDelta(inputData, PutCallFlag.Put), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesGammaUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = .39888;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholesGamma(inputData), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesVegaUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = 16.52798;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholesVega(inputData), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesThetaCallUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = -5.21103;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholesTheta(inputData, PutCallFlag.Call), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesThetaPutUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = -4.61402;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholesTheta(inputData, PutCallFlag.Put), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesRhoCallUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = 12.63174;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholesRho(inputData, PutCallFlag.Call), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BlackScholesRhoPutUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = -17.21863;
            var inputData = new BlackScholesInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.BlackScholesRho(inputData, PutCallFlag.Put), 5);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void PriceAtMaturityUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            Double expected = 480.36923;
            var inputData = new MonteCarloInputData(58.6, 60.0, .5, .01, .3);
            Double actual = Math.Round(calculations.PriceAtMaturity(inputData, 10.0), 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MonteCarloUsingValidData_ReturnsExpected()
        {
            var calculations = new Calculations();
            var inputData = new MonteCarloInputData(58.6, 60.0, .5, .01, .3);
            var random = new System.Random();
            List<Double> randomData = new List<double>();
            for (int i = 0; i < 1000; i++)
            {
                randomData.Add(random.NextDouble());
            }

            Double actual = Math.Round(calculations.MonteCarlo(inputData, randomData), 5);
            var greaterThanFour = actual > 4.0;
            var lessThanFive = actual < 5.0;

            Assert.AreEqual(true, greaterThanFour);
            Assert.AreEqual(true, lessThanFive);
        }

    }
}
