using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CannedBytes.CommandLineGui.Model;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// A custom DataTemplateSelector to select the correct DataTemplate for the <see cref="SingleValueBindingModel"/>.
    /// </summary>
    class ControlTemplateSelector : DataTemplateSelector
    {
        /// <summary>Bound to the DataTemplate for the Group Options control.</summary>
        public DataTemplate GroupOptionsControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Group Options Container control.</summary>
        public DataTemplate GroupOptionsContainerControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Check control.</summary>
        public DataTemplate CheckControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Folder control.</summary>
        public DataTemplate FolderControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the InputFile control.</summary>
        public DataTemplate InputFileControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Integer control.</summary>
        public DataTemplate IntegerControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Literal control.</summary>
        public DataTemplate LiteralControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Multiline control.</summary>
        public DataTemplate MultilineControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Number control.</summary>
        public DataTemplate NumberControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the OutputFile control.</summary>
        public DataTemplate OutputFileControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the SelectType control.</summary>
        public DataTemplate SelectTypeControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Text control.</summary>
        public DataTemplate TextControlTemplate { get; set; }

        /// <summary>Bound to the DataTemplate for the Uri control.</summary>
        public DataTemplate UriControlTemplate { get; set; }

        /// <summary>
        /// Called to select the correct DataTemplate for the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The Data bound item to select a DataTemplate for.</param>
        /// <param name="container">The container control the DataTemplate is presented in.</param>
        /// <returns>Returns null to resume the default (type-based) DataTemplate selection process.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dataTemplate = base.SelectTemplate(item, container);

            if (dataTemplate == null && item != null)
            {
                var singleValueBinding = item as SingleValueBindingModel;

                if (singleValueBinding != null)
                {
                    switch (singleValueBinding.Control.Type)
                    {
                        case ControlTypes.Check:
                            dataTemplate = CheckControlTemplate;
                            break;
                        case ControlTypes.Folder:
                            dataTemplate = FolderControlTemplate;
                            break;
                        case ControlTypes.InputFile:
                            dataTemplate = InputFileControlTemplate;
                            break;
                        case ControlTypes.Integer:
                            dataTemplate = IntegerControlTemplate;
                            break;
                        case ControlTypes.Literal:
                            dataTemplate = LiteralControlTemplate;
                            break;
                        case ControlTypes.Multiline:
                            dataTemplate = MultilineControlTemplate;
                            break;
                        case ControlTypes.Number:
                            dataTemplate = NumberControlTemplate;
                            break;
                        case ControlTypes.OutputFile:
                            dataTemplate = OutputFileControlTemplate;
                            break;
                        case ControlTypes.SelectType:
                            dataTemplate = SelectTypeControlTemplate;
                            break;
                        case ControlTypes.Text:
                            dataTemplate = TextControlTemplate;
                            break;
                        case ControlTypes.Uri:
                            dataTemplate = UriControlTemplate;
                            break;
                    }
                }

                var ogbm = item as OptionsGroupBindingModel;

                if (ogbm != null)
                {
                    bool hasSubGroups = ogbm.GroupBindings.Any();

                    if (hasSubGroups)
                    {
                        dataTemplate = GroupOptionsContainerControlTemplate;
                    }
                    else
                    {
                        dataTemplate = GroupOptionsControlTemplate;
                    }
                }
            }

            return dataTemplate;
        }
    }
}