#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Wpf;
using OxyPlot;

#endregion

namespace DotaWins
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public PlayerDisplay PlayerDisplays { get; set; }

        //   public IList<DataPoint> Points { get; set; }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PlayerDisplays = new PlayerDisplay();

            await PlayerDisplays.UpdateAsync(txtPlayerId.Text, 7);

            UpdateWinLossGraph(PlayerDisplays.Data.WinLosses.Reverse());
            gpmGraph.ItemsSource = GetPoints(PlayerDisplays.Data.GPM);
            xpmGraph.ItemsSource = GetPoints(PlayerDisplays.Data.XPM);
            //not best solution, but having a chart already made and making it equal new chart didn't work
            grdKDA.Children.Add(CreateKda(PlayerDisplays.Data.AverageKills, PlayerDisplays.Data.AverageDeaths,
                PlayerDisplays.Data.AverageAssists));


            lblWR_D.Content = PlayerDisplays.Data.Winrate;
            lblADuration_D.Content = PlayerDisplays.Data.AverageDuration;

            lblAAssists_D.Content = PlayerDisplays.Data.AverageAssists;
            lblADeaths_D.Content = PlayerDisplays.Data.AverageDeaths;
            lblAKills_D.Content = PlayerDisplays.Data.AverageKills;

            lblAXPM_D.Content = PlayerDisplays.Data.AverageXPM;
            lblAGPM_D.Content =  PlayerDisplays.Data.AverageGPM;

            lblAHeroDamage_D.Content = PlayerDisplays.Data.AverageHeroDamage;
            lblATowerDamage_D.Content = PlayerDisplays.Data.AverageTowerDamage;
            lblAHeroHealing_D.Content =PlayerDisplays.Data.AverageHeroHealing;
            lblALastHits_D.Content = PlayerDisplays.Data.AverageLastHits;
        }

        private static CartesianChart CreateKda(float kills, float deaths, float assists)
        {
            var tempChart = new CartesianChart();
            tempChart.AxisX.Add(new Axis {Labels = new[] {"Kills", "Deaths", "Assists"}});

            tempChart.AxisY.Add(new Axis());

            var seriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<float> {kills, deaths, assists}
                }
            };
            tempChart.Series = seriesCollection;
            return tempChart;
        }

        private static IEnumerable<DataPoint> GetPoints(IEnumerable<float> pointsList)
        {
            var points = new List<DataPoint>();

            var x = 0;
            foreach (var pm in pointsList)
            {
                points.Add(new DataPoint(x, pm));
                x++;
            }
            return points;
        }


        private void UpdateWinLossGraph(IEnumerable<int> winLoseList)
        {
            var points = new List<DataPoint>();

            var x = 0;
            var currentWl = 0;
            foreach (var outcome in winLoseList)
            {
                currentWl += outcome;
                points.Add(new DataPoint(x, currentWl));
                x++;
            }
            winLossGraph.ItemsSource = points;
        }
    }
}