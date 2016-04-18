using System.Windows.Controls;
using System.Windows.Data;
using CannedBytes.CommandLineGui.Commands;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for UriControl.xaml
    /// </summary>
    partial class UriControl : GuiControl
    {
        public UriControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        protected override BindingExpression GetBindingExpressionForValidation()
        {
            return this.Uri.GetBindingExpression(TextBox.TextProperty);
        }

        private void Uri_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}