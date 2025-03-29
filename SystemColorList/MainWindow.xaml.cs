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
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace SystemColorList;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static readonly DependencyProperty BrushItemsProperty = DependencyProperty.Register(
        nameof(BrushItems), typeof(ObservableCollection<BrushItem>), typeof(MainWindow),
        new PropertyMetadata(new ObservableCollection<BrushItem>()));

    public ObservableCollection<BrushItem> BrushItems
    {
        get => (ObservableCollection<BrushItem>)GetValue(BrushItemsProperty);
        set => SetValue(BrushItemsProperty, value);
    }

    public MainWindow()
    {
        InitializeComponent();

        // Get all system colors as BrushItem objects
        var systemBrushItems = typeof(SystemColors).GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Where(p => p.PropertyType == typeof(Color))
            .Select(p => new BrushItem(p.Name, p.GetValue(null)));

        // Add system colors to the collection
        BrushItems = [.. systemBrushItems];

        Debug.WriteLine($"System colors count: {BrushItems.Count}");
        Debug.WriteLine($"System colors: {string.Join(", ", BrushItems.Select(c => c.Name))}");
    }

    public record BrushItem(string Name, Brush Brush)
    {
        public BrushItem(string name, object? color) : this(name, Brushes.Transparent)
        {
            if (color is Color c)
            {
                Brush = new SolidColorBrush(c);
            }
        }
    }
}