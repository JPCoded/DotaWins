#region

using System.Windows;

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
     

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PlayerDisplays = new PlayerDisplay();
         
   
           
            PlayerDisplays.Update(txtPlayerId.Text, 7);

            foreach (var outcome in PlayerDisplays.Data.WinLosses)
            {
               
              //  txtResults.Text += outcome + "\n";
            }
        }


    }
}