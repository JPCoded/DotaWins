using System.Collections.Generic;
using OxyPlot;

namespace DotaWins
{
   public class MainViewModel
    {
        public MainViewModel()
        {
            Title = "Win/Loss";
            Points = new List<DataPoint>();
        }

        public string Title { get; }

        public IList<DataPoint> Points { get; set; }
    }
}

