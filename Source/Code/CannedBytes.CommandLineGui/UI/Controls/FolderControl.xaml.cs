using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CannedBytes.CommandLineGui.Commands;
using CannedBytes.CommandLineGui.Model;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for FolderControl.xaml
    /// </summary>
    partial class FolderControl : GuiControl
    {
        public FolderControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        protected override BindingExpression GetBindingExpressionForValidation()
        {
            return this.FolderPath.GetBindingExpression(TextBox.TextProperty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fbd = ControlFactory.CreateFolderBrowserDialog(
                "Select a folder.");

            if (fbd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
            {
                SingleValueBindingModel.Value.Value = fbd.SelectedPath;
            }
        }

        private void FolderPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}