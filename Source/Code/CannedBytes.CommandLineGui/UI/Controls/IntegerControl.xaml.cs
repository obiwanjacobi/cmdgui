using System.Windows.Controls;
using CannedBytes.CommandLineGui.Commands;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for IntegerControl.xaml
    /// </summary>
    partial class IntegerControl : GuiControl
    {
        public IntegerControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        private void Integer_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}