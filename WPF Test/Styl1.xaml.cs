using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Test
{
    /// <summary>
    /// Styl1.xaml の相互作用ロジック
    /// </summary>
    public partial class Styl1 : Page
    {
        public Styl1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            button2.IsEnabled = true;
            tb.IsReadOnly = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            button2.IsEnabled = false;
            tb.IsReadOnly = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tbb.IsEnabled = !tbb.IsEnabled;
            pb.IsEnabled = !pb.IsEnabled;
        }
    }
}
