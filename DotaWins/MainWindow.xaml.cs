#region

using System.Collections.Generic;
using System.Windows;
using OxyPlot;

#endregion

namespace DotaWins
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public PlayerDisplay PlayerDisplays { get; set; }

        public IList<DataPoint> Points { get; set; }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PlayerDisplays = new PlayerDisplay();

            Points = new List<DataPoint>();

            var x = 0;
            var currentWL = 0;
            PlayerDisplays.Update(txtPlayerId.Text, 7);

            foreach (var outcome in PlayerDisplays.Data.WinLosses)
            {
                
                currentWL += outcome;
              Points.Add(new DataPoint(x,currentWL));
                x++;
            }
            lineSeries.ItemsSource = Points;
        }


    }
}