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
    /// Cont3.xaml の相互作用ロジック
    /// </summary>
    public partial class Cont3 : Page
    {
        public Cont3()
        {
            InitializeComponent();
        }

        private void ContentControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is ContentControl control)
            {
                control.ContentTemplate = control.FindResource("B") as DataTemplate;
            }
        }

        private void ContentControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is ContentControl control)
            {
                control.ContentTemplate = control.FindResource("A") as DataTemplate;
            }
        }
    }
}
