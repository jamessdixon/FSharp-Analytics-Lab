using NewCo.OptionsTradingProgram.Data;
using NewCo.OptionsTradingProgram.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace NewCo.OptionsTradingProgram.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            FileSystemStockProvider provider = new FileSystemStockProvider(@"C:\Data\TEST.txt");
            var stockPrices = provider.GetData().Take(20);
            this.StockPriceDataGrid.ItemsSource = stockPrices;

            var adjustedClosePrices = from stockPrice in stockPrices
                    select stockPrice.Item7;

            var dates = from stockPrice in stockPrices.Skip(2)
                                     select new { stockPrice.Item1 };

            var calculations = new Calculations();
            var movingAverage = calculations.MovingAverage(adjustedClosePrices, 3);
            var movingAverages = dates.Zip(movingAverage, (d, p) => new { date=d.Item1, price=p});

            var bollingerBands = calculations.BollingerBands(adjustedClosePrices, 3);
            var upperBandBands = dates.Zip(bollingerBands, (d, bb) => new { date = d.Item1, upperBand = bb.Item1 + (bb.Item2 * 2) });
            var lowerBandBands = dates.Zip(bollingerBands, (d, bb) => new { date = d.Item1, lowerBand = bb.Item1 + (bb.Item2 * 2) * -1 });

            this.stockPriceLineGraph.DependentValuePath = "price";
            this.stockPriceLineGraph.IndependentValuePath = "date";
            this.stockPriceLineGraph.ItemsSource = movingAverages;

            this.stockPriceLineGraph2.DependentValuePath = "upperBand";
            this.stockPriceLineGraph2.IndependentValuePath = "date";
            this.stockPriceLineGraph2.ItemsSource = upperBandBands;

            this.stockPriceLineGraph3.DependentValuePath = "lowerBand";
            this.stockPriceLineGraph3.IndependentValuePath = "date";
            this.stockPriceLineGraph3.ItemsSource = lowerBandBands;

            var latestPrice = stockPrices.Last();
            var adjustedClose = latestPrice.Item7;
            var closestDollar = Math.Round(adjustedClose, 0);

            var theGreeks = new List<GreekData>();
            for (int i = 0; i < 5; i++)
            {
                var greekData = new GreekData();
                greekData.StrikePrice = closestDollar - i;
                theGreeks.Add(greekData);
                greekData = new GreekData();
                greekData.StrikePrice = closestDollar + i;
                theGreeks.Add(greekData);
            }
            theGreeks.Sort((greek1,greek2)=>greek1.StrikePrice.CompareTo(greek2.StrikePrice));

            foreach (var greekData in theGreeks)
            {
                var inputData =
                    new BlackScholesInputData(adjustedClose, greekData.StrikePrice, .5, .01, .3);
                greekData.DeltaCall = calculations.BlackScholesDelta(inputData, PutCallFlag.Call);
                greekData.DeltaPut = calculations.BlackScholesDelta(inputData, PutCallFlag.Put);
                greekData.Gamma = calculations.BlackScholesGamma(inputData);
                greekData.RhoCall = calculations.BlackScholesRho(inputData, PutCallFlag.Call);
                greekData.RhoPut = calculations.BlackScholesRho(inputData, PutCallFlag.Put);
                greekData.ThetaCall = calculations.BlackScholesTheta(inputData, PutCallFlag.Call);
                greekData.ThetaPut = calculations.BlackScholesTheta(inputData, PutCallFlag.Put);
                greekData.Vega = calculations.BlackScholesVega(inputData);

            }

            this.TheGreeksDataGrid.ItemsSource = theGreeks;


            var blackScholes = new List<BlackScholesData>();
            for (int i = 0; i < 5; i++)
            {
                var blackScholesData = new BlackScholesData();
                blackScholesData.StrikePrice = closestDollar - i;
                blackScholes.Add(blackScholesData);
                blackScholesData = new BlackScholesData();
                blackScholesData.StrikePrice = closestDollar + i;
                blackScholes.Add(blackScholesData);
            }
            blackScholes.Sort((bsmc1, bsmc2) => bsmc1.StrikePrice.CompareTo(bsmc2.StrikePrice));

            var random = new System.Random();
            List<Double> randomData = new List<double>();
            for (int i = 0; i < 1000; i++)
            {
                randomData.Add(random.NextDouble());
            }

            foreach (var blackScholesMonteCarlo in blackScholes)
            {
                var blackScholesInputData =
                    new BlackScholesInputData(adjustedClose, blackScholesMonteCarlo.StrikePrice, .5, .01, .3);
                var monteCarloInputData =
                    new MonteCarloInputData(adjustedClose, blackScholesMonteCarlo.StrikePrice, .5, .01, .3);

                blackScholesMonteCarlo.Call = calculations.BlackScholes(blackScholesInputData, PutCallFlag.Call);
                blackScholesMonteCarlo.Put = calculations.BlackScholes(blackScholesInputData, PutCallFlag.Put);
                blackScholesMonteCarlo.MonteCarlo = calculations.MonteCarlo(monteCarloInputData, randomData);
            }

            this.BlackScholesDataGrid.ItemsSource = blackScholes;
        
        }

    }

   
}
