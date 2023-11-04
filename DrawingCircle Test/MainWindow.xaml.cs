using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DrawingCircle_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            VM = (MWVM)DataContext;
        }

        private MWVM VM { get; }

        private bool LD;
        private Point LP;

        private bool RD;
        private Point RP;

        private void left_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LD = true;
            LP = e.GetPosition(left);
            VM.LX = LP.X;
            VM.LY = LP.Y;
            VM.LW = 0;
            VM.LH = 0;
            VM.L1X = LP.X - 5;
            VM.L1Y = LP.Y - 5;
        }

        private void left_MouseMove(object sender, MouseEventArgs e)
        {
            if (LD)
            {
                Point p = e.GetPosition(left);
                double lw = p.X - LP.X;
                double lh = p.Y - LP.Y;
                if (lw < 0)
                {
                    VM.LX = p.X;
                    VM.LW = -lw;
                }
                else
                {
                    VM.LW = lw;
                }
                if (lh < 0)
                {
                    VM.LY = p.Y;
                    VM.LH = -lh;
                }
                else
                {
                    VM.LH = lh;
                }
                VM.L2X = p.X - 5;
                VM.L2Y = p.Y - 5;
                Debug.WriteLine($"{VM.LX}, {VM.LY}, {VM.LW}, {VM.LH}");
            }
        }

        private void left_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LD = false;
        }

        private void right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RD = true;
            RP = e.GetPosition(right);
            VM.RX = RP.X;
            VM.RY = RP.Y;
            VM.RW = 0;
            VM.RH = 0;
            VM.R1X = RP.X - 5;
            VM.R1Y = RP.Y - 5;
        }

        private void right_MouseMove(object sender, MouseEventArgs e)
        {
            if (RD)
            {
                Point p = e.GetPosition(right);
                double rw = p.X - RP.X;
                double rh = p.Y - RP.Y;
                if (rw < 0)
                {
                    VM.RX = RP.X + rw;
                    VM.RW = rw * -2;
                }
                else
                {
                    VM.RX = RP.X - rw;
                    VM.RW = rw * 2;
                }
                if (rh < 0)
                {
                    VM.RY = RP.Y + rh * 2;
                    VM.RH = rh * -2;
                }
                else
                {
                    VM.RY = RP.Y;
                    VM.RH = rh * 2;
                }
                VM.R2X = p.X - 5;
                VM.R2Y = p.Y - 5;
                Debug.WriteLine($"{VM.RX}, {VM.RY}, {VM.RW}, {VM.RH}");
            }
        }

        private void right_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RD = false;
        }
    }
}
