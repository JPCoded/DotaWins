#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using OxyPlot;
using OxyPlot.Series;
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
            GPMGraph.ItemsSource = GetPoints(PlayerDisplays.Data.GPM);
            XPMGraph.ItemsSource = GetPoints(PlayerDisplays.Data.XPM);
            UpdateTest(PlayerDisplays.Data.GPM);
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

      
        private void UpdateTest(IEnumerable<float> gpmlist)
        {
            var ch = new CartesianChart();

          var  crtGpm = new ChartValues<float>();

            crtGpm.AddRange(gpmlist);

            ch.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = crtGpm
                }
            };
            testGrid.Children.Add(ch);

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