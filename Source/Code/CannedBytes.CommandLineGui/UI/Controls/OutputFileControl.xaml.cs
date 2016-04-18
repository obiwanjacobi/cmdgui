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
    /// Interaction logic for OutputFileControl.xaml
    /// </summary>
    partial class OutputFileControl : GuiControl
    {
        public OutputFileControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        protected override BindingExpression GetBindingExpressionForValidation()
        {
            return this.OutputFilePath.GetBindingExpression(TextBox.TextProperty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mask = ValueBindingModel.Control.Properties.Mask();

            var fm = new FileFilterManager();
            fm.AddParsed(mask);
            fm.AddAllFilesFilter();

            var sfd = ControlFactory.CreateSaveFileDialog("Select an output file.", fm.ToString());

            if (sfd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
            {
                SingleValueBindingModel.Value.Value = sfd.FileName;
            }
        }

        private void OutputFilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}