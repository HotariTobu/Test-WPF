using System;
using System.Collections.Generic;
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

namespace Indexed1_Bitmap_Test
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

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            Point point = e.GetPosition((IInputElement)sender);

            int pointX = (int)(point.X * PixelWidth / image.ActualWidth);
            int pointY = (int)(point.Y * PixelHeight / image.ActualHeight);

            int index = pointY * PixelWidth + pointX;
            byte pixel = Pixels[index];

            System.Diagnostics.Debug.WriteLine($"{point} | ({pointX}, {pointY}) | {pixel}");

            if (Mask == null)
            {
                return;
            }

            /*Int32Rect rect = new Int32Rect(pointX - 1, pointY - 1, 3, 3);
            bool[] pixels = Enumerable.Repeat(true, 9).ToArray();
            int stride = GetStride(3, 1);
            Mask.WritePixels(rect, pixels, 1, 0);*/

            //bool[] ps1 = new bool[PixelWidth * PixelHeight];
            //Mask.CopyPixels(ps1, Mask.BackBufferStride, 0);

            //byte[] ps2 = new byte[PixelWidth * PixelHeight];
            //Mask.CopyPixels(ps2, Mask.BackBufferStride, 0);

            byte[] buffer = new byte[PixelWidth * PixelHeight / 8];
            System.Runtime.InteropServices.Marshal.Copy(Mask.BackBuffer, buffer, 0, buffer.Length);

            Mask.Lock();
            unsafe
            {
                /*bool* pixelPointer = (bool*)Mask.BackBuffer.ToPointer();

                pixelPointer += (pointY - 1) * Mask.BackBufferStride + (pointX - 1);
                int interval = Mask.BackBufferStride - 3;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        *pixelPointer = true;
                        pixelPointer++;
                    }

                    pixelPointer += interval;
                }*/

                // x / 8
                // >> x % 8 

                byte* bufferPointer = (byte*)Mask.BackBuffer.ToPointer();

                int leftTopX = pointX - 1;
                byte bits1 = (byte)(224 >> (leftTopX % 8));
                byte bits2 = (byte)(224 << (8 - leftTopX % 8));

                bufferPointer += (pointY - 1) * Mask.BackBufferStride + leftTopX / 8;

                for (int i = 0; i < 3; i++)
                {
                    bufferPointer[0] |= bits1;
                    bufferPointer[1] |= bits2;

                    bufferPointer += Mask.BackBufferStride;
                }
            }
            Mask.AddDirtyRect(new Int32Rect(pointX - 1, pointY - 1, 3, 3));
            //Mask.AddDirtyRect(new Int32Rect(0, 0, PixelWidth, PixelHeight));
            Mask.Unlock();
        }

        private byte[] Pixels = new byte[0];
        private int PixelWidth;
        private int PixelHeight;

        private WriteableBitmap? Mask;

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Image image = (Image)sender;
            ImageSource source = image.Source;
            if (source is BitmapSource bitmapSource)
            {
                FormatConvertedBitmap bitmap = new FormatConvertedBitmap(bitmapSource, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
                byte[] pixels = new byte[bitmap.PixelWidth * bitmap.PixelHeight];
                int stride = bitmap.PixelWidth * bitmap.Format.BitsPerPixel / 8;
                bitmap.CopyPixels(pixels, stride, 0);

                Pixels = pixels;
                PixelWidth = bitmap.PixelWidth;
                PixelHeight = bitmap.PixelHeight;

                Mask = new WriteableBitmap(PixelWidth, PixelHeight, 96, 96, PixelFormats.Indexed1, new BitmapPalette(new Color[] { Colors.Transparent, Colors.Red, }));
                MaskImage.Source = Mask;
            }
        }
    }
}
