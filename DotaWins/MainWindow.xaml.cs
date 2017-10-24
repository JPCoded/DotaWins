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

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PlayerDisplays = new PlayerDisplay();

            await PlayerDisplays.UpdateAsync(txtPlayerId.Text, 7);

            UpdateWinLossGraph(PlayerDisplays.Data.WinLosses.Reverse());
            
            lblAXPM_.Content = PlayerDisplays.Data.AverageXPM;
            lblAGPM_.Content = PlayerDisplays.Data.AverageGPM;
            lblWR_.Content = $"{PlayerDisplays.Data.Winrate:P}";
            lblAAssists_.Content = $"{PlayerDisplays.Data.AverageAssists:F1}";
            lblADeaths_.Content = $"{PlayerDisplays.Data.AverageDeaths:F1}"; 
            lblAKills_.Content = $"{PlayerDisplays.Data.AverageKills:F1}";
            lblADuration_.Content = PlayerDisplays.Data.AverageDuration;



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