using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CannedBytes.CommandLineGui.Commands;
using CannedBytes.CommandLineGui.Model;
using CannedBytes.CommandLineGui.Model.Factory;
using CannedBytes.CommandLineGui.Persistence;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for InputFileControl.xaml
    /// </summary>
    partial class InputFileControl : GuiControl
    {
        public InputFileControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        protected override BindingExpression GetBindingExpressionForValidation()
        {
            return this.InputFilePath.GetBindingExpression(TextBox.TextProperty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mask = ValueBindingModel.Control.Properties.Mask();

            var fm = new FileFilterManager();
            fm.AddParsed(mask);
            fm.AddAllFilesFilter();

            var ofd = ControlFactory.CreateOpenFileDialog("Select an input file.", fm.ToString());

            if (ofd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
            {
                SingleValueBindingModel.Value.Value = ofd.FileName;
            }
        }

        private void InputFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}