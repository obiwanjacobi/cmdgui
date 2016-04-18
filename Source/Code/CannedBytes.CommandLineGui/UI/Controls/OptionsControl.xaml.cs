using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CannedBytes.CommandLineGui.Commands;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for OptionsControl.xaml
    /// </summary>
    partial class OptionsControl : GuiControl
    {
        public OptionsControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        protected override BindingExpression GetBindingExpressionForValidation()
        {
            return this.OptionsList.GetBindingExpression(ListBox.SelectedItemProperty);
        }

        private void DeselectOption_Click(object sender, RoutedEventArgs e)
        {
            this.OptionsList.SelectedIndex = -1;
        }

        private void OptionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}