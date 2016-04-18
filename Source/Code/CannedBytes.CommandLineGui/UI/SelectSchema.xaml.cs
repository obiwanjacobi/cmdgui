using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CannedBytes.CommandLineGui.Persistence;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// Interaction logic for SelectSchema.xaml
    /// </summary>
    partial class SelectSchema : UserControl
    {
        public SelectSchema()
        {
            InitializeComponent();

            DataContext = this;
        }

        public IEnumerable<GuiSchemaInfo> ExecutableNames { get; set; }

        public GuiSchemaInfo Selected { get; set; }

        public string SchemaPath { get; set; }

        public bool IsNewSchema { get; set; }

        private void BrowseForSchema_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var fm = new FileFilterManager();
            fm.AddGuiSchemaFilter();

            var ofd = ControlFactory.CreateOpenFileDialog("Select a schema file.", fm.ToString());

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.NewSchemaPath.Text = ofd.FileName;
                SchemaPath = ofd.FileName;
            }

            this.NewSchemaOption.IsChecked = true;
        }

        private void OK_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsNewSchema && Selected == null)
            {
                // no selection made
                return;
            }

            var wnd = this.Parent as Window;

            if (wnd != null)
            {
                wnd.DialogResult = true;
                wnd.Close();
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadedSchemaOption.IsChecked = true;
        }

        private void NewSchemaPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.NewSchemaOption.IsChecked = true;
        }
    }

    class GuiSchemaInfo
    {
        public string SchemaFilePath { get; set; }

        public string ExecutableName { get; set; }

        public string ScreenName { get; set; }
    }
}