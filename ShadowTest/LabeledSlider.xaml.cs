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

namespace ShadowTest
{
    /// <summary>
    /// LabeledSlider.xaml の相互作用ロジック
    /// </summary>
    public partial class LabeledSlider : UserControl
    {
        public LabeledSlider()
        {
            InitializeComponent();
        }

        #region == Text ==

        public string Text { get => GetValue(TextProperty) as string; set => SetValue(TextProperty, value); }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabeledSlider), new PropertyMetadata(string.Empty));

        #endregion

        #region == Maximum ==

        public double Maximum { get => (double)GetValue(MaximumProperty); set => SetValue(MaximumProperty, value); }
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(LabeledSlider), new PropertyMetadata(100d));

        #endregion
        #region == Minimum ==

        public double Minimum { get => (double)GetValue(MinimumProperty); set => SetValue(MinimumProperty, value); }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(LabeledSlider), new PropertyMetadata(0d));

        #endregion
        #region == Value ==

        public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(LabeledSlider), new PropertyMetadata(0d));

        #endregion

    }
}
