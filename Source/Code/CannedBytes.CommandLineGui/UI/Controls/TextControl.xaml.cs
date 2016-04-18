using System.Windows.Controls;
using System.Windows.Data;
using CannedBytes.CommandLineGui.Commands;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for TextControl.xaml
    /// </summary>
    partial class TextControl : GuiControl
    {
        public TextControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        protected override BindingExpression GetBindingExpressionForValidation()
        {
            return this.Text.GetBindingExpression(TextBox.TextProperty);
        }

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}