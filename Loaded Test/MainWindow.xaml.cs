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

namespace Loaded_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            window1 = new Window1();
            window1.DataContext = "vm";
        }

        private Window1 window1;

        private void SHOWButton_Click(object sender, RoutedEventArgs e)
        {
            window1.Show();
        }

        private void HIDEButton_Click(object sender, RoutedEventArgs e)
        {
            window1.Hide();
        }
    }
}
