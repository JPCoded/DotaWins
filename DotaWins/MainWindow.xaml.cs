#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using OxyPlot;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using VerticalAlignment = System.Windows.VerticalAlignment;

#endregion

namespace DotaWins
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Matrix _wtoDMatrix, _dtoWMatrix;

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
           
            UpdateGpmGraph(PlayerDisplays.Data.GXPM);
            UpdateXpmGraph(PlayerDisplays.Data.GXPM);
            NewGraph(ref canGraph,PlayerDisplays.Data.GXPM);
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

        public Canvas NewGraph(ref Canvas canvasToGraph,IEnumerable<dynamic> dataset, IEnumerable<dynamic> dataset2 = null, double wxmin = -1, double wxmax = 101,double wymin = -1, double wymax = 11)
        {
         //   var canvasToGraph = new Canvas();
    
            

            const double xstep = 10;
            const double ystep = 1;

            const double dmargin = 10;
            const double dxmin = dmargin;
            var dxmax = canGraph.Width - dmargin;
            const double dymin = dmargin;
            var dymax = canGraph.Height - dmargin;

            // Prepare the transformation matrices.
            PrepareTransformations(
                wxmin, wxmax, wymin, wymax,
                dxmin, dxmax, dymax, dymin);

            // Get the tic mark lengths.
            var p0 = DtoW(new Point(0, 0));
            var p1 = DtoW(new Point(5, 5));
            var xtic = p1.X - p0.X;
            var ytic = p1.Y - p0.Y;

            // Make the X axis.
            var xaxisGeom = new GeometryGroup();
            p0 = new Point(wxmin, 0);
            p1 = new Point(wxmax, 0);
            xaxisGeom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));

            for (var x = xstep; x <= wxmax - xstep; x += xstep)
            {
                // Add the tic mark.
                var tic0 = WtoD(new Point(x, -ytic));
                var tic1 = WtoD(new Point(x, ytic));
                xaxisGeom.Children.Add(new LineGeometry(tic0, tic1));

                // Label the tic mark's X coordinate.
                DrawText(canvasToGraph, x.ToString(),
                    new Point(tic0.X, tic0.Y + 5), 12,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top);
            }

            var xaxisPath = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = xaxisGeom
            };

           canvasToGraph.Children.Add(xaxisPath);

            // Make the Y axis.
            var yaxisGeom = new GeometryGroup();
            p0 = new Point(0, wymin);
            p1 = new Point(0, wymax);
            xaxisGeom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));

            for (var y = ystep; y <= wymax - ystep; y += ystep)
            {
                // Add the tic mark.
                var tic0 = WtoD(new Point(-xtic, y));
                var tic1 = WtoD(new Point(xtic, y));
                xaxisGeom.Children.Add(new LineGeometry(tic0, tic1));

                // Label the tic mark's Y coordinate.
                DrawText(canvasToGraph, y.ToString(),
                    new Point(tic0.X - 10, tic0.Y), 12,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center);
            }

            var yaxisPath = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = yaxisGeom
            };

            canvasToGraph.Children.Add(yaxisPath);

            // Make some data sets.
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            var rand = new Random();
            for (var dataSet = 0; dataSet < 3; dataSet++)
            {
                double lastY = rand.Next(3, 7);

                var points = new PointCollection();
                for (double x = 0; x <= 100; x += 10)
                {
                    lastY += rand.Next(-10, 10) / 10.0;
                    if (lastY < 0) lastY = 0;
                    if (lastY > 10) lastY = 10;
                    var p = new Point(x, lastY);
                    points.Add(WtoD(p));
                }

                var polyline = new Polyline
                {
                    StrokeThickness = 1,
                    Stroke = brushes[dataSet],
                    Points = points
                };

                canvasToGraph.Children.Add(polyline);
            }

            // Make a title
            var titleLocation = WtoD(new Point(50, 10));
            DrawText(canvasToGraph, "Amazing Data", titleLocation, 20,
                HorizontalAlignment.Center,
                VerticalAlignment.Top);

            return canvasToGraph;
        }
        public void graphtest()
        {
            double wxmin = -1;
            double wxmax = 101;
            double wymin = -1;
            double wymax = 11;
            const double xstep = 10;
            const double ystep = 1;

            const double dmargin = 10;
            const double dxmin = dmargin;
            var dxmax = canGraph.Width - dmargin;
            const double dymin = dmargin;
            var dymax = canGraph.Height - dmargin;

            // Prepare the transformation matrices.
            PrepareTransformations(
                wxmin, wxmax, wymin, wymax,
                dxmin, dxmax, dymax, dymin);

            // Get the tic mark lengths.
            var p0 = DtoW(new Point(0, 0));
            var p1 = DtoW(new Point(5, 5));
            var xtic = p1.X - p0.X;
            var ytic = p1.Y - p0.Y;

            // Make the X axis.
            var xaxisGeom = new GeometryGroup();
            p0 = new Point(wxmin, 0);
            p1 = new Point(wxmax, 0);
            xaxisGeom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));

            for (var x = xstep; x <= wxmax - xstep; x += xstep)
            {
                // Add the tic mark.
                var tic0 = WtoD(new Point(x, -ytic));
                var tic1 = WtoD(new Point(x, ytic));
                xaxisGeom.Children.Add(new LineGeometry(tic0, tic1));

                // Label the tic mark's X coordinate.
                DrawText(canGraph, x.ToString(),
                    new Point(tic0.X, tic0.Y + 5), 12,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top);
            }

            var xaxisPath = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = xaxisGeom
            };

            canGraph.Children.Add(xaxisPath);

            // Make the Y axis.
            var yaxisGeom = new GeometryGroup();
            p0 = new Point(0, wymin);
            p1 = new Point(0, wymax);
            xaxisGeom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));

            for (var y = ystep; y <= wymax - ystep; y += ystep)
            {
                // Add the tic mark.
                var tic0 = WtoD(new Point(-xtic, y));
                var tic1 = WtoD(new Point(xtic, y));
                xaxisGeom.Children.Add(new LineGeometry(tic0, tic1));

                // Label the tic mark's Y coordinate.
                DrawText(canGraph, y.ToString(),
                    new Point(tic0.X - 10, tic0.Y), 12,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center);
            }

            var yaxisPath = new Path
            {
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Data = yaxisGeom
            };

            canGraph.Children.Add(yaxisPath);

            // Make some data sets.
            Brush[] brushes = {Brushes.Red, Brushes.Green, Brushes.Blue};
            var rand = new Random();
            for (var dataSet = 0; dataSet < 3; dataSet++)
            {
                double lastY = rand.Next(3, 7);

                var points = new PointCollection();
                for (double x = 0; x <= 100; x += 10)
                {
                    lastY += rand.Next(-10, 10) / 10.0;
                    if (lastY < 0) lastY = 0;
                    if (lastY > 10) lastY = 10;
                    var p = new Point(x, lastY);
                    points.Add(WtoD(p));
                }

                var polyline = new Polyline
                {
                    StrokeThickness = 1,
                    Stroke = brushes[dataSet],
                    Points = points
                };

                canGraph.Children.Add(polyline);
            }

            // Make a title
            var titleLocation = WtoD(new Point(50, 10));
            DrawText(canGraph, "Amazing Data", titleLocation, 20,
                HorizontalAlignment.Center,
                VerticalAlignment.Top);
        }

        private void PrepareTransformations(
            double wxmin, double wxmax, double wymin, double wymax,
            double dxmin, double dxmax, double dymin, double dymax)
        {
            // Make WtoD.
            _wtoDMatrix = Matrix.Identity;
            _wtoDMatrix.Translate(-wxmin, -wymin);

            var xscale = (dxmax - dxmin) / (wxmax - wxmin);
            var yscale = (dymax - dymin) / (wymax - wymin);
            _wtoDMatrix.Scale(xscale, yscale);

            _wtoDMatrix.Translate(dxmin, dymin);

            // Make DtoW.
            _dtoWMatrix = _wtoDMatrix;
            _dtoWMatrix.Invert();
        }

        // Transform a point from world to device coordinates.
        private Point WtoD(Point point) => _wtoDMatrix.Transform(point);

        // Transform a point from device to world coordinates.
        private Point DtoW(Point point) => _dtoWMatrix.Transform(point);

        // Position a label at the indicated point.
        private static void DrawText(Panel can, string text, Point location,
            double fontSize,
            HorizontalAlignment halign, VerticalAlignment valign)
        {
            // Make the label.
            var canvasLabel = new Label
            {
                Content = text,
                FontSize = fontSize
            };
            can.Children.Add(canvasLabel);

            // Position the label.
            canvasLabel.Measure(new Size(double.MaxValue, double.MaxValue));

            var x = location.X;
            if (halign == HorizontalAlignment.Center)
            {
                x -= canvasLabel.DesiredSize.Width / 2;
            }
            else if (halign == HorizontalAlignment.Right)
            {
                x -= canvasLabel.DesiredSize.Width;
            }

            Canvas.SetLeft(canvasLabel, x);

            var y = location.Y;
            if (valign == VerticalAlignment.Center)
            {
                y -= canvasLabel.DesiredSize.Height / 2;
            }
            else if (valign == VerticalAlignment.Bottom)
            {
                y -= canvasLabel.DesiredSize.Height;
            }

            Canvas.SetTop(canvasLabel, y);
        }
    }
}