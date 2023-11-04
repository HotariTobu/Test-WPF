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

namespace BindingResourceTest
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // 参照オブジェクトのプロパティ変更
            // → StaticResource, DynamicResource両方が影響を受ける
            SolidColorBrush brush = this.FindResource("testBrush") as SolidColorBrush;
            brush.Color = Colors.Blue;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // リソースオブジェクトの差し替え
            // DynamicResourceのみ影響を受ける
            this.Resources["testBrush"] = new SolidColorBrush(Colors.Green);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // これじゃなにもかわんないよ。
            var resource = this.Resources["testBrush"] as SolidColorBrush;
            resource = new SolidColorBrush(Colors.Green);
        }
    }
}
