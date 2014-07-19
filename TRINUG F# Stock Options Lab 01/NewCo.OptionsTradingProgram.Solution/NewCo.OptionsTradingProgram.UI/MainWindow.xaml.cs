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
        }
    }

   
}
