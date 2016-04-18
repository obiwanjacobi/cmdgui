using CannedBytes.CommandLineGui.Commands;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for GroupOptionsControl.xaml
    /// </summary>
    partial class GroupOptionsControl : GuiControl
    {
        public GroupOptionsControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        private void Deselect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GroupOptionsList.SelectedIndex = -1;
        }

        private void GroupOptionsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}