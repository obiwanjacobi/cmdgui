using System.Windows.Controls;
using CannedBytes.CommandLineGui.Commands;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for MultilineControl.xaml
    /// </summary>
    partial class MultilineControl : GuiControl
    {
        public MultilineControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        private void MulitlineText_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}