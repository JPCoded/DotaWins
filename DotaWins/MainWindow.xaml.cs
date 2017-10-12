using System;
using System.Windows;

namespace DotaWins
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

        public PlayerDisplay PlayerDisplays { get; set; }
        public int RunningRetrievals { get; set; }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PlayerDisplays = new PlayerDisplay();
            PlayerDisplay.RetrievalStarted += PlayerDisplay_RetrievalStarted;
            PlayerDisplay.RetrievalCompleted += PlayerDisplay_RetrievalCompleted;
            PlayerDisplays.Update(txtPlayerId.Text,7);

            foreach (var outcome in PlayerDisplays.Data.WinLosses)
            {
                txtResults.Text += outcome + "\n";
            }
        }

        private void PlayerDisplay_RetrievalStarted(object sender, EventArgs e)
        {
            RunningRetrievals++;
        }

        private void PlayerDisplay_RetrievalCompleted(object sender, EventArgs e)
        {
            RunningRetrievals--;
           
        }

    }
}
