using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mouse_Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel ViewModel;
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = (MainWindowViewModel)DataContext;
        }

        List<StylusPoint> points = new List<StylusPoint>();

        private void InkCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (ViewModel.IsFloat)
            {
                /*IC.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = MouseDownEvent,
                    Source = IC,
                });*/
                Point point = e.GetPosition(IC);
                points.Add(new StylusPoint(point.X, point.Y));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsFloat)
            {
                Stroke stroke = new Stroke(new StylusPointCollection(points));
                Matrix matrix = new Matrix();
                matrix.Translate(100, -50);
                var preps = stroke.GetBezierStylusPoints();
                stroke.Transform(matrix, false);
                var newps = stroke.GetBezierStylusPoints();
                IC.Strokes.Add(stroke);
                points.Clear();
            }
        }

        private void IC_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                StrokeCollection strokes = IC.Strokes;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StrokeCollection strokes = IC.Strokes;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string data = string.Join("\n\n", IC.Strokes.Select(stroke =>
               {
                   return string.Join("\n", stroke.StylusPoints.Select(point => $"new Point({point.X}, {point.Y}),"));
               }));
            Clipboard.SetText(data);
        }
    }
}
