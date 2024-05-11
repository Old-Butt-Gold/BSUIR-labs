using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace MIAPR_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         const int PointsCount = 10000;

         double _pc1;

         double _pc2;

         readonly Random _random = new();

        public MainWindow()
        {
            InitializeComponent();
            SliderPC1.ValueChanged += SliderPC1_OnValueChanged;
            SliderPC2.ValueChanged += SliderPC2_OnValueChanged;
        }

        void SliderPC1_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderPC2.Value = 1 - SliderPC1.Value;
            ReDrawChart();
        }

        void SliderPC2_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderPC1.Value = 1 - SliderPC2.Value;
        }

        void ReDrawChart()
        {
            _pc1 = SliderPC1.Value;
            _pc2 = SliderPC2.Value;
            Canvas.Source = new DrawingImage(Calc());
        }

        double[] GetPoints(int minValue, int maxValue)
        {
            double[] points = new double[PointsCount];
            for (int i = 0; i < points.Length; i++)
                points[i] = _random.Next(minValue, maxValue - 50); //50?
            return points;
        }
        
        public double CalculateMathExpectation(double[] randomVariables) => randomVariables.Sum() / randomVariables.Length;
        
        public double CalculateStandardDeviation(double[] randomVariables, double mathExpectation)
        {
            var res = randomVariables.Select(x => Math.Pow(x - mathExpectation, 2)).Sum();
            return Math.Sqrt(res / randomVariables.Length);
        }
        
        public double CalculateProbabilityDensity(double x, double mathExpectation, double standardDeviation)
        {
            double numerator = Math.Exp(-0.5 * Math.Pow((x - mathExpectation) / standardDeviation, 2));
            double denominator = standardDeviation * Math.Sqrt(2 * Math.PI);
            return numerator / denominator;
        }
        
        DrawingGroup Calc()
        {
            DrawingGroup drawingGroup = new DrawingGroup();
            
            var points1 = GetPoints(-100, (int) (BorderImage.ActualWidth / 1.6));
            var points2 = GetPoints(100, (int) (BorderImage.ActualWidth / 1.6) + 200);

            double mx1 = CalculateMathExpectation(points1);
            double mx2 = CalculateMathExpectation(points2);

            var sigma1 = CalculateStandardDeviation(points1, mx1);
            var sigma2 = CalculateStandardDeviation(points2, mx2);
            
            double[] probabilityDensityForFirstRandomVariables = new double[(int) BorderImage.ActualWidth];
            double[] probabilityDensityForSecondRandomVariables = new double[(int) BorderImage.ActualWidth];

            for (int i = 0; i < (int) BorderImage.ActualWidth; i++)
            {
                probabilityDensityForFirstRandomVariables[i] = CalculateProbabilityDensity(-100 + i, mx1, sigma1) * _pc1;
                probabilityDensityForSecondRandomVariables[i] = CalculateProbabilityDensity(-100 + i, mx2, sigma2) * _pc2;
            }

            SolidColorBrush[] brushes = { new (Colors.Red), new(Colors.Blue), new(Colors.White)};
            GeometryGroup[] geometryGroups = { new(), new(), new()};
            for (var x = 1; x < probabilityDensityForFirstRandomVariables.Length; x++)
            {
                var blueLineGeometry = new LineGeometry(
                    new Point(x - 1, (BorderImage.ActualHeight - (int) (probabilityDensityForFirstRandomVariables[x - 1] * BorderImage.ActualHeight * 470))),
                    new Point(x, (BorderImage.ActualHeight - (int) (probabilityDensityForFirstRandomVariables[x] * BorderImage.ActualHeight * 470))));
                var redLineGeometry = new LineGeometry(
                    new Point(x - 1, (BorderImage.ActualHeight - (int) (probabilityDensityForSecondRandomVariables[x - 1] * BorderImage.ActualHeight * 470))),
                    new Point(x, (BorderImage.ActualHeight - (int) (probabilityDensityForSecondRandomVariables[x] * BorderImage.ActualHeight * 470))));

                geometryGroups[1].Children.Add(blueLineGeometry);
                geometryGroups[0].Children.Add(redLineGeometry);
            }

            DrawXY(geometryGroups[2]);

            int intersectionPointIndex = -1;
            double intersectionThreshold = 0.000003;

            for (int x = 1; x < probabilityDensityForFirstRandomVariables.Length; x++)
            {
                double difference = Math.Abs(probabilityDensityForFirstRandomVariables[x] - probabilityDensityForSecondRandomVariables[x]);
                if (difference < intersectionThreshold)
                {
                    intersectionPointIndex = x;
                    break;
                }
            }
            
            if (intersectionPointIndex != -1)
            {
                drawingGroup.Children.Add(new GeometryDrawing(Brushes.Green, new Pen(Brushes.Green, 2),
                        new LineGeometry(new Point(intersectionPointIndex, 0), new Point(intersectionPointIndex, BorderImage.ActualHeight))));
                var ellipse = new GeometryGroup();
                ellipse.Children.Add(new EllipseGeometry(new (intersectionPointIndex, BorderImage.ActualHeight - (probabilityDensityForSecondRandomVariables[intersectionPointIndex] * BorderImage.ActualHeight * 470)), 5.5, 5.5));
                drawingGroup.Children.Add(new GeometryDrawing(Brushes.Green, new Pen(Brushes.Green, 3), ellipse));
            }


            var error1 = probabilityDensityForSecondRandomVariables.Take(intersectionPointIndex).Sum();
            var error2 = probabilityDensityForFirstRandomVariables.Skip(intersectionPointIndex).Sum();
            TextBoxFalseAlarm.Text = error1.ToString("F10");
            TextBoxMiss.Text = error2.ToString("F10");
            TextBoxAmountOfRisk.Text = (error1 + error2).ToString("F10");

            
            drawingGroup.Children.Add(new GeometryDrawing(brushes[1], new Pen(brushes[1], 2), geometryGroups[1]));
            drawingGroup.Children.Add(new GeometryDrawing(brushes[0], new Pen(brushes[0], 2), geometryGroups[0]));
            drawingGroup.Children.Add(new GeometryDrawing(brushes[2], new Pen(brushes[2], 2), geometryGroups[2]));
            return drawingGroup;            
        }

        void DrawXY(GeometryGroup geometryGroup)
        {
            var blackLineX = new LineGeometry(new Point(0, BorderImage.ActualHeight),
                new Point(BorderImage.ActualWidth, BorderImage.ActualHeight));
            var blackLineTopArrow = new LineGeometry(new Point(BorderImage.ActualWidth, BorderImage.ActualHeight),
                new Point(BorderImage.ActualWidth - 15, BorderImage.ActualHeight - 5));
            var blackLineBottomArrow = new LineGeometry(
                new Point(BorderImage.ActualWidth, BorderImage.ActualHeight),
                new Point(BorderImage.ActualWidth - 15, BorderImage.ActualHeight + 5));
            var blackLineY = new LineGeometry(new Point(0, BorderImage.ActualHeight - 1),
                new Point(0, 0));

            var blackLineLeftArrow = new LineGeometry(new Point(0, 0), new Point(-5, 15));
            var blackLineRightArrow = new LineGeometry(new Point(0, 0), new Point(5, 15));

            geometryGroup.Children.Add(blackLineX);
            geometryGroup.Children.Add(blackLineTopArrow);
            geometryGroup.Children.Add(blackLineBottomArrow);
            geometryGroup.Children.Add(blackLineY);
            geometryGroup.Children.Add(blackLineLeftArrow);
            geometryGroup.Children.Add(blackLineRightArrow);
        }
    }
}