using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Loaded_Test
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Window_Loaded");
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Window_Unloaded");
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Debug.WriteLine("Window_ContentRendered");
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Debug.WriteLine("Window_Activated");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Debug.WriteLine("Window_Closed");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Debug.WriteLine("Window_Closing");
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Debug.WriteLine("Window_Deactivated");
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Debug.WriteLine("Window_Initialized");
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("Window_IsVisibleChanged");
        }

        private void Window_LayoutUpdated(object sender, EventArgs e)
        {
            //Debug.WriteLine("Window_LayoutUpdated");
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Debug.WriteLine("Window_SourceInitialized");
        }

        private void Window_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            Debug.WriteLine("Window_SourceUpdated");
        }

        private void Window_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            Debug.WriteLine("Window_TargetUpdated");
        }

        private void Window_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            Debug.WriteLine("Window_ManipulationStarted");
        }

        private void Window_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            Debug.WriteLine("Window_ManipulationStarting");
        }
    }
}
