#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using OxyPlot;
using LineSeries = LiveCharts.Wpf.LineSeries;

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
            UpdateKda(PlayerDisplays.Data.AverageKills, PlayerDisplays.Data.AverageDeaths,PlayerDisplays.Data.AverageAssists);
          

            lblWR_D.Content = $"{PlayerDisplays.Data.Winrate:P}";
            lblADuration_D.Content = PlayerDisplays.Data.AverageDuration;

            lblAAssists_D.Content = $"{PlayerDisplays.Data.AverageAssists:F1}";
            lblADeaths_D.Content = $"{PlayerDisplays.Data.AverageDeaths:F1}";
            lblAKills_D.Content = $"{PlayerDisplays.Data.AverageKills:F1}";

            lblAXPM_D.Content = $"{PlayerDisplays.Data.AverageXPM:F1}";
            lblAGPM_D.Content = $"{PlayerDisplays.Data.AverageGPM:F1}";

            lblAHeroDamage_D.Content = $"{PlayerDisplays.Data.AverageHeroDamage:F1}";
            lblATowerDamage_D.Content = $"{PlayerDisplays.Data.AverageTowerDamage:F1}";
            lblAHeroHealing_D.Content = $"{PlayerDisplays.Data.AverageHeroHealing:F1}";
            lblALastHits_D.Content = $"{PlayerDisplays.Data.AverageLastHits:F1}";
        }
      
        private void UpdateKda(float kills, float deaths, float assists)
        {
          
            kdaChart.AxisX.Add( new Axis ());
           kdaChart.AxisY.Add( new Axis());
            kdaChart.LegendLocation = LegendLocation.Bottom;
            var seriesCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "K",
                    Values = new ChartValues<float> { kills }
                },
                new RowSeries
                {
                    Title = "D",
                    Values = new ChartValues<float>{deaths}
                },
                new RowSeries
                {
                    Title = "A",
                    Values = new ChartValues<float>{assists}
                }
            };
          
            kdaChart.Series = seriesCollection;

       


        }
        private IEnumerable<DataPoint> GetPoints(IEnumerable<float> pointsList)
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
            var Points = new List<DataPoint>();

            var x = 0;
            var currentWl = 0;
            foreach (var outcome in winLoseList)
            {
                currentWl += outcome;
                Points.Add(new DataPoint(x, currentWl));
                x++;
            }
            winLossGraph.ItemsSource = Points;
        }
    }
}