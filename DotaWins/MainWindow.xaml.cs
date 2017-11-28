#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
            //DataContext = this;
        }
       public ChartValues<float> crtGPM { get; set; }
        public PlayerDisplay PlayerDisplays { get; set; }

        public IList<DataPoint> Points { get; set; }
    
        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

            PlayerDisplays = new PlayerDisplay();

            await PlayerDisplays.UpdateAsync(txtPlayerId.Text, 7);

            UpdateWinLossGraph(PlayerDisplays.Data.WinLosses.Reverse());
           
            UpdateGpmGraph(PlayerDisplays.Data.GXPM);
            UpdateXpmGraph(PlayerDisplays.Data.GXPM);
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
            crtGPM = new ChartValues<float>();
          crtGPM.AddRange(gpmlist);
          
        }
        private void UpdateGpmGraph(IEnumerable<float[]> gpmList)
        {
            Points = new List<DataPoint>();

            var x = 0;
            foreach (var pm in gpmList)
            {
                Points.Add(new DataPoint(x, pm[0]));

                x++;
            }
            GPMGraph.ItemsSource = Points;
        }

        private void UpdateXpmGraph(IEnumerable<float[]> xpmList)
        {
            Points = new List<DataPoint>();

            var x = 0;
            foreach (var pm in xpmList)
            {
                Points.Add(new DataPoint(x, pm[1]));

                x++;
            }
            XPMGraph.ItemsSource = Points;
        }

        private void UpdateWinLossGraph(IEnumerable<int> winLoseList)
        {
            Points = new List<DataPoint>();

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