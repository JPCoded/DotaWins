using System.Collections.Generic;
using OxyPlot;

namespace DotaWins
{
    internal class PointsClass
    {
        public static IEnumerable<DataPoint> GetPoints(IEnumerable<float> pointsList)
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

        private static IEnumerable<DataPoint> GetPoints(IEnumerable<int> pointsList)
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

        public static IEnumerable<DataPoint> GetWinLossPoints(IEnumerable<int> winLoseList)
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
            return points;
        }
    }
}