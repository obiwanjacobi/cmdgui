using CannedBytes.CommandLineGui.Model;
using System;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    /// <summary>
    /// Interaction logic for LiteralControl.xaml
    /// </summary>
    partial class LiteralControl : GuiControl
    {
        public LiteralControl()
        {
            InitializeComponent();
            InitializeGrid(LayoutGrid);
        }

        protected override void OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            // when the DataContext is set...
            if (e.Property.Name == "DataContext")
            {
                var bindingModel = DataContext as SingleValueBindingModel;

                if (bindingModel != null)
                {
                    // if there is no custom display value, show the description of the argument.
                    if (String.IsNullOrEmpty(bindingModel.Value.DisplayValue))
                    {
                        bindingModel.Value.DisplayValue = bindingModel.Description;
                    }
                }
            }
        }
    }
}