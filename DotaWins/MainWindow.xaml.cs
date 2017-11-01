#region

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
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
        }

        public PlayerDisplay PlayerDisplays { get; set; }

        public IList<DataPoint> Points { get; set; }
        public IList<DataPoint> Points2 { get; set; }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PlayerDisplays = new PlayerDisplay();

            await PlayerDisplays.UpdateAsync(txtPlayerId.Text, 7);

            UpdateWinLossGraph(PlayerDisplays.Data.WinLosses.Reverse());
           UpdateGxpmGraph(PlayerDisplays.Data.GXPM);
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

        private void UpdateGxpmGraph(IEnumerable<float[]> gxpmList)
        {
            Points = new List<DataPoint>();
            Points2 = new List<DataPoint>();
            var x = 0;
            foreach (var pm in gxpmList)
            {
                Points.Add(new DataPoint(x,pm[0]));
                Points2.Add(new DataPoint(x,pm[1]));
                x++;
            }
            GXPMGraph.ItemsSource = Points;
            GXPMGraph.ItemsSource = Points2;
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
            lineSeries.ItemsSource = Points;
        }
    }
}