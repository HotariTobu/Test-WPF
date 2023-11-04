using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Windows.Data.Pdf;

namespace PDF_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MWVM VM;

        public MainWindow()
        {
            InitializeComponent();

            VM = (MWVM)DataContext;
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

                if (path.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            VM.Path = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            VM.PageSources = new ObservableCollection<ImageSource>(await PDFLoader.LoadPDF(VM.Path));
            GC.Collect();
        }
    }
}
