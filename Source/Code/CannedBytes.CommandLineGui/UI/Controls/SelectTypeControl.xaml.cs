using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using CannedBytes.CommandLineGui.Commands;
using CannedBytes.CommandLineGui.Model;
using CannedBytes.CommandLineGui.Model.Factory;
using CannedBytes.CommandLineGui.Persistence;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for SelectTypeControl.xaml
    /// </summary>
    partial class SelectTypeControl : GuiControl
    {
        public SelectTypeControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        public ObservableCollection<TypeInfo> Types { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mask = ValueBindingModel.Control.Properties.Mask();

            var fm = new FileFilterManager();
            fm.AddParsed(mask);
            fm.AddAssmbliesFilter();
            fm.AddAllFilesFilter();

            var ofd = ControlFactory.CreateOpenFileDialog("Select an assembly file.", fm.ToString());

            if (ofd.ShowDialog(new Win32Window(App.Current.MainWindow)) == System.Windows.Forms.DialogResult.OK)
            {
                var types = AssemblyLoader.LoadTypes(ofd.FileName);

                Types = new ObservableCollection<TypeInfo>(types);

                OnPropertyChanged("Types");
            }
        }

        private void TypeList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            AppCommands.Modified.Execute(null, this);
        }

        public class TypeInfo
        {
            public string FullName { get; set; }

            public string FullyQualifiedName { get; set; }
        }

        //---------------------------------------------------------------------

        private static class AssemblyLoader
        {
            public static IEnumerable<TypeInfo> LoadTypes(string assemblyPath)
            {
                var assembly = Assembly.LoadFile(assemblyPath);

                var types = from type in assembly.GetTypes()
                            where type.IsPublic
                            select new TypeInfo { FullName = type.FullName, FullyQualifiedName = type.AssemblyQualifiedName };

                return types;
            }
        }
    }
}