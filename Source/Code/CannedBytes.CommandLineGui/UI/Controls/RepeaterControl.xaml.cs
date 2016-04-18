using System.Windows;
using System.Windows.Controls;
using CannedBytes.CommandLineGui.Model;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for RepeaterControl.xaml
    /// </summary>
    partial class RepeaterControl : GuiControl
    {
        public RepeaterControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        public RepeatingValueBindingModel RepeatingValueBindingModel
        {
            get { return DataContext as RepeatingValueBindingModel; }
        }

        private void RemoveRepeaterItem_Click(object sender, RoutedEventArgs e)
        {
            var bindingModel = ((Button)sender).DataContext as ValueBindingModel;

            if (bindingModel != null)
            {
                RepeatingValueBindingModel.Bindings.Remove(bindingModel);
            }
        }

        private void AddRepeaterItem_Click(object sender, RoutedEventArgs e)
        {
            RepeatingValueBindingModel.Add();
        }
    }
}