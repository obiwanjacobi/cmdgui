using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CannedBytes.CommandLineGui.Model;

namespace CannedBytes.CommandLineGui.UI.Controls
{
    class GuiControl : UserControl, INotifyPropertyChanged
    {
        public GuiControl()
        {
            Loaded += new RoutedEventHandler(GuiControl_Loaded);
        }

        private void GuiControl_Loaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as BindingModel;

            if (model != null &&
                model.Control != null &&
                model.Control.ValidationRules != null)
            {
                InitializeValidation(model.Control.ValidationRules);
            }
        }

        protected virtual void InitializeValidation(IEnumerable<ValidationRule> rules)
        {
            if (rules == null) return;

            var expression = GetBindingExpressionForValidation();

            if (expression != null)
            {
                foreach (var rule in rules)
                {
                    rule.ValidatesOnTargetUpdated = true;

                    expression.ParentBinding.ValidationRules.Add(rule);
                }

                // trigger validation
                expression.UpdateTarget();
            }
            else
            {
                Trace.WriteLine("ValidationRules were not set.", "Validation");
            }
        }

        protected virtual BindingExpression GetBindingExpressionForValidation()
        {
            return null;
        }

        protected void InitializeGrid(Grid grid)
        {
            if (grid != null)
            {
                //if (grid.ColumnDefinitions.Count > 0)
                //    grid.ColumnDefinitions[0].Width = new GridLength(0.25, GridUnitType.Star);
                if (grid.ColumnDefinitions.Count > 0)
                    grid.ColumnDefinitions[0].SharedSizeGroup = "CtrlLblCol";
                if (grid.ColumnDefinitions.Count > 1)
                    grid.ColumnDefinitions[1].Width = new GridLength(0.75, GridUnitType.Star);
                if (grid.ColumnDefinitions.Count > 2)
                    grid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Auto);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected BindingModel BindingModel
        {
            get { return DataContext as BindingModel; }
        }

        protected ValueBindingModel ValueBindingModel
        {
            get { return DataContext as ValueBindingModel; }
        }

        protected SingleValueBindingModel SingleValueBindingModel
        {
            get { return DataContext as SingleValueBindingModel; }
        }
    }
}