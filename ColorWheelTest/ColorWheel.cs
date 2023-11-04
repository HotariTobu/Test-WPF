using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorWheelTest
{
    public class ColorWheel : Control
    {
        static ColorWheel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorWheel), new FrameworkPropertyMetadata(typeof(ColorWheel)));
        }

        #region == Radius ==

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(ColorWheel), new PropertyMetadata(0d, (d, e) =>
        {
            if (d is ColorWheel colorWheel)
            {
                double radius = (double)e.NewValue;
                double diameter = radius * 2;
                colorWheel.Width = diameter;
                colorWheel.Height = diameter;
                colorWheel.Clip = new EllipseGeometry(new Point(radius, radius), radius, radius);
            }
        }));
        public double Radius { get => (double)GetValue(RadiusProperty); set => SetValue(RadiusProperty, value); }

        #endregion
        #region == Quality ==

        public int Quality { get => (int)GetValue(QualityProperty); set => SetValue(QualityProperty, value); }
        public static readonly DependencyProperty QualityProperty = DependencyProperty.Register("Quality", typeof(int), typeof(ColorWheel), new PropertyMetadata(0, (d, e) =>
        {
            if (d is ColorWheel colorWheel)
            {
                colorWheel.Source = DrawColorWheel(colorWheel.Quality);
            }
        }));

        #endregion

        #region == Source ==

        private static readonly DependencyPropertyKey SourcePropertyKey = DependencyProperty.RegisterReadOnly("Source", typeof(ImageSource), typeof(ColorWheel), new PropertyMetadata());
        public static readonly DependencyProperty SourceProperty = SourcePropertyKey.DependencyProperty;
        public ImageSource Source { get => (ImageSource)GetValue(SourceProperty); private set => SetValue(SourcePropertyKey, value); }

        #endregion

        private static readonly double TwicePI = 2 * Math.PI;

        private static ImageSource DrawColorWheel(int radius)
        {
            int size = radius * 2;
            WriteableBitmap writeableBitmap = new WriteableBitmap(size, size, 96, 96, PixelFormats.Bgr24, null);

            int length = (int)(TwicePI * size);
            Color[] colors = new Color[length];
            for (int i = 0; i < length; i++)
            {
                colors[i] = GetColorFromHSV((double)i / length, 1, 1);
            }

            byte[] pixels = new byte[size * size * 3];
            for (int y = 0; y < size; y++)
            {
                int disY = y - radius;
                for (int x = 0; x < size; x++)
                {
                    int index = (size * y + x) * 3;
                    Color color = colors[(int)((Math.Atan2(disY, x - radius) + Math.PI) / TwicePI * length) - 1];
                    pixels[index] = color.B;
                    pixels[index + 1] = color.G;
                    pixels[index + 2] = color.R;
                }
            }

            writeableBitmap.WritePixels(new Int32Rect(0, 0, size, size), pixels, size * 3, 0, 0);
            return writeableBitmap;
        }

        private static Color GetColorFromHSV(double h, double s, double v)
        {
            h = h < 1 ? h : 0;
            double max = v;
            double range = s * max;
            double min = max - range;
            double[] rgb = new double[3];
            int hd = (int)(h * 6) % 6;
            int hdd = (int)(h * 3) % 3;
            rgb[hdd] = max;
            rgb[((hd + 1) / 2 + 1) % 3] = min;
            rgb[(5 - hd) % 3] = (hd % 2 == 0 ? -1 : 1) * (h * 6 - hdd * 2 - 1) * range + min;
            return Color.FromRgb((byte)(rgb[0] * 255), (byte)(rgb[1] * 255), (byte)(rgb[2] * 255));
        }
    }
}
