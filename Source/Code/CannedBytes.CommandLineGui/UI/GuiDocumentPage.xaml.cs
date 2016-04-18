using System.Windows;
using System.Windows.Controls;
using CannedBytes.CommandLineGui.Commands;
using CannedBytes.CommandLineGui.Persistence;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// Interaction logic for GuiDocumentPage.xaml
    /// </summary>
    partial class GuiDocumentPage : UserControl
    {
        public GuiDocumentPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(GuiDocumentPage_Loaded);
        }

        void GuiDocumentPage_Loaded(object sender, RoutedEventArgs e)
        {
            var guiDocument = DataContext as GuiDocument;

            if (guiDocument != null)
            {
                // reset the dirty flag when the page is loaded.
                guiDocument.IsChanged = false;
            }
        }

        private void BrowseLocation_Click(object sender, RoutedEventArgs e)
        {
            var fm = new FileFilterManager();
            fm.Add("*.dll;*.exe", "Assembly files (*.dll, *.exe)");
            fm.AddAllFilesFilter();

            var ofd = ControlFactory.CreateOpenFileDialog("Browse to the tool executable file.",
                                                        fm.ToString());

            if (ofd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
            {
                this.ToolLocation.Text = ofd.FileName;
            }
        }

        private void RestoreLocation_Click(object sender, RoutedEventArgs e)
        {
            var guiDocument = DataContext as GuiDocument;

            if (guiDocument != null)
            {
                guiDocument.ToolInfo.RevertToolExecutionPath();
            }
        }

        private void ToolLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }
    }
}