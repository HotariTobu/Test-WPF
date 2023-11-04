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

namespace RTBTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MWVM VM;
        private TextRange TextRange => new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);

        public MainWindow()
        {
            InitializeComponent();
            VM = (MWVM)DataContext;
        }

        private void RTB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextSelection selection = RTB.Selection;

            Paragraph paragraph = new();
            RTB.Document.Blocks.Add(paragraph);

            VM.StartIndex = getIndex(selection.Start);
            VM.Length = selection.Start.GetOffsetToPosition(selection.End);

            VM.InsertionIndex = getIndex(new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd).Start.GetNextContextPosition(LogicalDirection.Forward));
            //VM.InsertionIndex = getIndex(RTB.Document.ContentStart.GetNextContextPosition(LogicalDirection.Forward));
            //VM.InsertionIndex = getIndex(selection.Start.GetInsertionPosition(LogicalDirection.Forward));
            VM.NextContentIndex = getIndex(selection.Start.GetNextContextPosition(LogicalDirection.Forward));
            //VM.NextInsertionIndex = getIndex(selection.Start.GetNextContextPosition(LogicalDirection.Forward).GetNextContextPosition(LogicalDirection.Forward));
            VM.NextInsertionIndex = getIndex(selection.Start.GetNextContextPosition(LogicalDirection.Forward).GetNextInsertionPosition(LogicalDirection.Forward));
            //VM.NextInsertionIndex = getIndex(selection.Start.GetNextContextPosition(LogicalDirection.Backward));
            //VM.NextInsertionIndex = getIndex(selection.Start.GetNextInsertionPosition(LogicalDirection.Forward));

            VM.ForwardRect = selection.Start.GetCharacterRect(LogicalDirection.Forward);
            VM.BackwardRect = selection.Start.GetCharacterRect(LogicalDirection.Backward);

            RTB.Document.Blocks.Remove(paragraph);

            int getIndex(TextPointer textPointer)
            {
                if (textPointer == null)
                {
                    return 0;
                }
                return TextRange.Start.GetOffsetToPosition(textPointer);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextPointer start = TextRange.Start.GetPositionAtOffset(VM.StartIndex, LogicalDirection.Forward);
            TextPointer end = start.GetPositionAtOffset(VM.Length, LogicalDirection.Backward);
            TextRange selected = new TextRange(start, end);
            selected.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
        }
    }
}
