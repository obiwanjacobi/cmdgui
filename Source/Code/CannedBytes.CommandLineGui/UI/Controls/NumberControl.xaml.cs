using System.Windows.Controls;
using CannedBytes.CommandLineGui.Commands;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for NumberControl.xaml
    /// </summary>
    partial class NumberControl : GuiControl
    {
        public NumberControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}