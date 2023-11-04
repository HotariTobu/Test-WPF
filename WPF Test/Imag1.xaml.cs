using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Test
{
    /// <summary>
    /// Imag1.xaml の相互作用ロジック
    /// </summary>
    public partial class Imag1 : Page
    {
        public Imag1()
        {
            InitializeComponent();

            efs(Environment.CurrentDirectory);
        }

        private void efs(string dire)
        {
            foreach (string path in Directory.GetFiles(dire))
            {
                ef(path);
            }
        }

        private async void ef(string path)
        {
            Image image = new Image();
            image.Width = 300;
            wrap.Children.Add(image);
            ImageSource imageSource = null;
            await Task.Run(() =>
            {
                //System.Threading.Thread.Sleep(5000);
                //bool re=Application.Current.Dispatcher.c
                ImageSource imageSource = IconUtilities.GetImageOf(path, IconUtilities.IconSize.Jumbo);
                image.Dispatcher.Invoke(() => image.Source = imageSource);
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            efs(ttb.Text);
        }
    }

    internal static class IconUtilities
    {
        #region == Size ==

        public enum IconSize
        {
            Large,
            Small,
            ExtraLarge,
            SystemSmall,
            Jumbo,
        };

        private static readonly int[] ActualSizes =
        {
            32,
            16,
            48,
            16,
            256,
        };

        #endregion

        #region == Image ==

        public static ImageSource GetImageThumbnail(string path, IconSize size)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return null;
            }

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path);
            bitmapImage.DecodePixelWidth = ActualSizes[(int)size];
            bitmapImage.EndInit();
            return bitmapImage;
        }

        #endregion

        #region == Video ==

        public static ImageSource GetVideoThumbnail(string path, IconSize size, int timeLimit = 1)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return null;
            }

            MediaPlayer mediaPlayer = new MediaPlayer { ScrubbingEnabled = true, IsMuted = true };
            mediaPlayer.Open(new Uri(path));
            mediaPlayer.Pause();

            int count = 0;
            while (mediaPlayer.NaturalVideoWidth == 0)
            {
                System.Threading.Thread.Sleep(100);
                count++;
                if (count == timeLimit * 10)
                {
                    return null;
                }
            }

            int actualSize = ActualSizes[(int)size];
            double naturalWidth = mediaPlayer.NaturalVideoWidth;
            double naturalHeight = mediaPlayer.NaturalVideoHeight;
            int width = actualSize;
            int height = actualSize;
            if (naturalWidth < naturalHeight)
            {
                width = (int)(actualSize * naturalWidth / naturalHeight);
            }
            else
            {
                height = (int)(actualSize * naturalHeight / naturalWidth);
            }

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawVideo(mediaPlayer, new Rect(new Size(width, height)));
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(drawingVisual);
            ImageSource imageSource = BitmapFrame.Create(renderTargetBitmap).GetCurrentValueAsFrozen() as BitmapFrame;

            mediaPlayer.Close();
            return imageSource;
        }

        #endregion

        #region == Others ==

        private static readonly ConcurrentDictionary<string, (IconSize, ImageSource)> FileIcons = new ConcurrentDictionary<string, (IconSize, ImageSource)>(EqualityComparer<string>.Default);

        public static ImageSource GetGeneralIcon(string path, IconSize size)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            if (File.Exists(path))
            {
                string extension = System.IO.Path.GetExtension(path);
                if (FileIcons.ContainsKey(extension))
                {
                    (IconSize, ImageSource) icon = FileIcons[extension];
                    if (icon.Item1 == size)
                    {
                        return icon.Item2;
                    }
                }

                ImageSource imageSource = GetIcon(path, size);
                FileIcons.TryAdd(extension, (size, imageSource));

                return imageSource;
            }
            else if (Directory.Exists(path))
            {
                return GetIcon(path, size);
            }

            return null;
        }

        private static ImageSource GetIcon(string path, IconSize size)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr result = IntPtr.Zero;
            Application.Current.Dispatcher.Invoke(() =>
            {
                result = SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), 0x4000);
                
            });
            if (result == IntPtr.Zero)
            {
                return null;
            }

            IImageList imageList = null;
            if (SHGetImageList((int)size, ref IID_IImageList, out imageList) != 0 || imageList == null)
            {
                return null;
            }

            IntPtr hIcon1 = IntPtr.Zero;
            if (imageList.GetIcon(shinfo.iIcon, 0x1, ref hIcon1) != 0 || hIcon1 == IntPtr.Zero)
            {
                return null;
            }

            IntPtr hIcon2 = System.Drawing.Icon.FromHandle(hIcon1).ToBitmap().GetHbitmap();
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(hIcon2, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hIcon1);
            DeleteObject(hIcon2);

            return imageSource;
        }

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shell32.dll")]
        private static extern int SHGetImageList(int iImageList, ref Guid riid, out IImageList ppvObj);

        private static Guid IID_IImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

        [ComImport()]
        [Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IImageList
        {
            int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);
            int ReplaceIcon(int i, IntPtr hicon, ref int pi);
            int SetOverlayImage(int iImage, int iOverlay);
            int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);
            int AddMasked(IntPtr hbmImage, int crMask, ref int pi);
            int Draw(ref IMAGELISTDRAWPARAMS pimldp);
            int Remove(int i);
            int GetIcon(int i, int flags, ref IntPtr picon);
        };

        private struct IMAGELISTDRAWPARAMS
        {
            private int cbSize;
            private IntPtr himl;
            private int i;
            private IntPtr hdcDst;
            private int x;
            private int y;
            private int cx;
            private int cy;
            private int xBitmap;
            private int yBitmap;
            private int rgbBk;
            private int rgbFg;
            private int fStyle;
            private int dwRop;
            private int fState;
            private int Frame;
            private int crEffect;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        #endregion

        #region == Mix ==

        private static readonly string[] ImageFormats =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".bmp",
            ".tiff",
            ".tif",
            ".ico",
            ".jpe",
            ".jfif",
            ".jfi",
            ".jif",
            ".dib",
            ".hdp",
            ".dp",
            ".dds",
        };

        private static readonly string[] VideoFormats =
        {
            ".mp4",
            ".mov",
            ".avi",
            ".wma",
            ".wmv",
            ".m4s",
            ".mpg",
            ".aa",
            ".ts",
            ".3gp",
            ".asf",
            ".m2p",
            ".m2ps",
            ".apng",
            ".gif",
            ".png",
            ".asf",
            ".dash",
            ".flv",
        };

        public static ImageSource GetImageOf(string path, IconSize size)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            if (File.Exists(path))
            {
                string extension = System.IO.Path.GetExtension(path);
                if (ImageFormats.Contains(extension, EqualityComparer<string>.Default))
                {
                    return GetImageThumbnail(path, size);
                }
                else if (VideoFormats.Contains(extension, EqualityComparer<string>.Default))
                {
                    return GetVideoThumbnail(path, size) ?? GetGeneralIcon(path, size);
                }
                else
                {
                    return GetGeneralIcon(path, size);
                }
            }
            else
            {
                return GetGeneralIcon(path, size);
            }
        }

        #endregion
    }

    public class WindowsShellAPI
    {
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public static extern int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

        //[DllImport("shell32.dll", EntryPoint = "#727")]
        //public static extern int SHGetImageList(SHIL iImageList, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, ref IImageList ppv);

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public static extern int SHGetImageList(SHIL iImageList, ref Guid riid, out IImageList ppv);

        //
        [DllImport("shell32.dll", EntryPoint = "#727")]
        public static extern int SHGetImageList(SHIL iImageList, ref Guid riid, out IntPtr ppv);

        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, int flags);

        //SHFILEINFO
        [Flags]
        public enum SHGFI
        {
            SHGFI_ICON = 0x000000100,
            SHGFI_DISPLAYNAME = 0x000000200,
            SHGFI_TYPENAME = 0x000000400,
            SHGFI_ATTRIBUTES = 0x000000800,
            SHGFI_ICONLOCATION = 0x000001000,
            SHGFI_EXETYPE = 0x000002000,
            SHGFI_SYSICONINDEX = 0x000004000,
            SHGFI_LINKOVERLAY = 0x000008000,
            SHGFI_SELECTED = 0x000010000,
            SHGFI_ATTR_SPECIFIED = 0x000020000,
            SHGFI_LARGEICON = 0x000000000,
            SHGFI_SMALLICON = 0x000000001,
            SHGFI_OPENICON = 0x000000002,
            SHGFI_SHELLICONSIZE = 0x000000004,
            SHGFI_PIDL = 0x000000008,
            SHGFI_USEFILEATTRIBUTES = 0x000000010,
            SHGFI_ADDOVERLAYS = 0x000000020,
            SHGFI_OVERLAYINDEX = 0x000000040
        }

        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }



        //IMAGE LIST
        public static Guid IID_IImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
        public static Guid IID_IImageList2 = new Guid("192B9D83-50FC-457B-90A0-2B82A8B5DAE1");
        //Private Const IID_IImageList    As String = "{46EB5926-582E-4017-9FDF-E8998DAA0950}"
        //Private Const IID_IImageList2   As String = "{192B9D83-50FC-457B-90A0-2B82A8B5DAE1}"


        [Flags]
        public enum SHIL
        {
            SHIL_JUMBO = 0x0004,
            SHIL_EXTRALARGE = 0x0002
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            int x;
            int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGEINFO
        {
            public IntPtr hbmImage;
            public IntPtr hbmMask;
            public int Unused1;
            public int Unused2;
            public RECT rcImage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGELISTDRAWPARAMS
        {
            public int cbSize;
            public IntPtr himl;
            public int i;
            public IntPtr hdcDst;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int xBitmap;    // x offest from the upperleft of bitmap
            public int yBitmap;    // y offset from the upperleft of bitmap
            public int rgbBk;
            public int rgbFg;
            public int fStyle;
            public int dwRop;
            public int fState;
            public int Frame;
            public int crEffect;
        }

        [Flags]
        public enum ImageListDrawItemConstants : int
        {
            /// <summary>
            /// Draw item normally.
            /// </summary>
            ILD_NORMAL = 0x0,
            /// <summary>
            /// Draw item transparently.
            /// </summary>
            ILD_TRANSPARENT = 0x1,
            /// <summary>
            /// Draw item blended with 25% of the specified foreground colour
            /// or the Highlight colour if no foreground colour specified.
            /// </summary>
            ILD_BLEND25 = 0x2,
            /// <summary>
            /// Draw item blended with 50% of the specified foreground colour
            /// or the Highlight colour if no foreground colour specified.
            /// </summary>
            ILD_SELECTED = 0x4,
            /// <summary>
            /// Draw the icon's mask
            /// </summary>
            ILD_MASK = 0x10,
            /// <summary>
            /// Draw the icon image without using the mask
            /// </summary>
            ILD_IMAGE = 0x20,
            /// <summary>
            /// Draw the icon using the ROP specified.
            /// </summary>
            ILD_ROP = 0x40,
            /// <summary>
            /// ?
            /// </summary>
            ILD_OVERLAYMASK = 0xF00,
            /// <summary>
            /// Preserves the alpha channel in dest. XP only.
            /// </summary>
            ILD_PRESERVEALPHA = 0x1000, // 
            /// <summary>
            /// Scale the image to cx, cy instead of clipping it.  XP only.
            /// </summary>
            ILD_SCALE = 0x2000,
            /// <summary>
            /// Scale the image to the current DPI of the display. XP only.
            /// </summary>
            ILD_DPISCALE = 0x4000
        }


        // interface COM IImageList
        [ComImportAttribute()]
        [GuidAttribute("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IImageList
        {
            [PreserveSig]
            int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);

            [PreserveSig]
            int ReplaceIcon(int i, IntPtr hicon, ref int pi);

            [PreserveSig]
            int SetOverlayImage(int iImage, int iOverlay);

            [PreserveSig]
            int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);

            [PreserveSig]
            int AddMasked(IntPtr hbmImage, int crMask, ref int pi);

            [PreserveSig]
            int Draw(ref IMAGELISTDRAWPARAMS pimldp);

            [PreserveSig]
            int Remove(int i);

            [PreserveSig]
            int GetIcon(int i, int flags, ref IntPtr picon);
        };
    }
}
