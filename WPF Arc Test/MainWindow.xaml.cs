using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Arc_Test
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

        private void SyntaxButton_Click(object sender, RoutedEventArgs e)
        {
            Matrix end = End.RenderTransform.Value;
            Matrix size = ArcSize.RenderTransform.Value;
            Clipboard.SetText($"A {size.OffsetX},{size.OffsetY} {RotationAngle.Value} {((IsClockwise.IsChecked ?? true) ? "1" : "0")} {((IsLargeArc.IsChecked ?? true) ? "1" : "0")} {end.OffsetX},{end.OffsetY}");
        }

        private void SegmentButton_Click(object sender, RoutedEventArgs e)
        {
            Matrix end = End.RenderTransform.Value;
            Matrix size = ArcSize.RenderTransform.Value;
            Clipboard.SetText($"<ArcSegment Point=\"{end.OffsetX},{end.OffsetY}\" RotationAngle=\"{RotationAngle.Value}\" SweepDirection=\"{((IsClockwise.IsChecked ?? true) ? SweepDirection.Clockwise : SweepDirection.Counterclockwise)}\" IsLargeArc=\"{((IsLargeArc.IsChecked ?? true) ? "True" : "False")}\" Size=\"{size.OffsetX},{size.OffsetY}\"/>");
        }
    }

    public class PointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Point point = Point.Parse(parameter as string);
                return new Point((double)values[0] + point.X, (double)values[1] + point.Y);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Size size = Size.Parse(parameter as string);
                return new Size((double)values[0] + size.Width, (double)values[1] + size.Height);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToSweepDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? SweepDirection.Clockwise : (bool)value ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
