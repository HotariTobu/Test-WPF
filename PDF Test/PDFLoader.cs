using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Streams;

namespace PDF_Test
{
    internal class PDFLoader
    {
        public static async Task<IEnumerable<ImageSource>> LoadPDF(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Enumerable.Empty<ImageSource>();
            }

            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return Enumerable.Empty<ImageSource>();
            }

            StorageFile file = await StorageFile.GetFileFromPathAsync(fileInfo.FullName);
            PdfDocument pdfDocument;

            try
            {
                pdfDocument = await PdfDocument.LoadFromFileAsync(file);
            }
            catch
            {
                return Enumerable.Empty<ImageSource>();
            }

            if (pdfDocument != null)
            {
                List<ImageSource> images = new List<ImageSource>();

                for (uint i = 0; i < pdfDocument.PageCount; i++)
                {
                    using (PdfPage page = pdfDocument.GetPage(i))
                    {
                        using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                        {
                            PdfPageRenderOptions renderOptions = new PdfPageRenderOptions();
                            renderOptions.DestinationWidth = (uint)Math.Round(page.Dimensions.ArtBox.Width / 96.0 * 200.0);
                            await page.RenderToStreamAsync(stream, renderOptions);
                            PngBitmapDecoder decoder = new PngBitmapDecoder(stream.AsStream(), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                            BitmapFrame bitmapFrame = decoder.Frames[0];
                            bitmapFrame.Freeze();
                            images.Add(bitmapFrame);
                        }
                    }
                }

                return images;
            }

            return Enumerable.Empty<ImageSource>();
        }
    }
}
