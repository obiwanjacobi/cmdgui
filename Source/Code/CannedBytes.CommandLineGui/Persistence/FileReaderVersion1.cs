using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CannedBytes.CommandLineGui.Model;
using CannedBytes.CommandLineGui.Persistence.Version1;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui.Persistence
{
    class FileReaderVersion1
    {
        public GuiFileDocument GuiFileDocument { get; private set; }

        public bool Read(string filePath)
        {
            var serializer = XmlSerializer<GuiFileDocument>.CreateFromResource(
                "CannedBytes.CommandLineGui.Persistence.Version1.CommandLineDocumentSchema.xsd");

            GuiFileDocument = serializer.Read(filePath);

            return (GuiFileDocument != null);
        }

        public bool Read(Stream input)
        {
            var serializer = XmlSerializer<GuiFileDocument>.CreateFromResource(
                "CannedBytes.CommandLineGui.Persistence.Version1.CommandLineDocumentSchema.xsd");

            GuiFileDocument = serializer.Read(input);

            return (GuiFileDocument != null);
        }

        public bool HasEmbededDefinition
        {
            get
            {
                return (GuiFileDocument != null &&
                    GuiFileDocument.ToolDefinition != null &&
                    GuiFileDocument.ToolDefinition.GuiSchema != null);
            }
        }

        public Executable GetEmbededToolDefinition()
        {
            if (HasEmbededDefinition)
            {
                using (var memStream = new MemoryStream(
                    Encoding.UTF8.GetBytes(GuiFileDocument.ToolDefinition.GuiSchema.OuterXml)))
                {
                    return XmlSerializer<Executable>.Deserialize(memStream);
                }
            }

            return null;
        }

        public void ApplyTo(GuiDocument document)
        {
            if (GuiFileDocument == null)
                throw new InvalidOperationException("Call Read before calling ApplyTo.");
            if (document.ToolBindingModel == null)
                throw new ArgumentException("The Binding Model must be filled out.", "document.ToolBindingModel");

            var commonParentFinder = new CommonParentFinder(GuiFileDocument.Arguments);
            commonParentFinder.Navigate(document.ToolBindingModel);

            var argApplyer = new BindingModelArgumentApplyer(GuiFileDocument.Arguments);
            argApplyer.CommonParent = commonParentFinder.CommonParent;

            argApplyer.Navigate(document.ToolBindingModel);
        }

        //---------------------------------------------------------------------

        private abstract class ArgumentBindingModelNavigator : BindingModelNavigator
        {
            protected ArgumentBindingModelNavigator(Persistence.Version1.ArgumentList arguments)
            {
                Arguments = arguments;
                NavigateAll = true;
            }

            public Persistence.Version1.ArgumentList Arguments { get; private set; }

            protected Persistence.Version1.Argument FindArgument(string name)
            {
                return (from arg in Arguments
                        where arg.Name == name
                        select arg).FirstOrDefault();
            }
        }

        private class CommonParentFinder : ArgumentBindingModelNavigator
        {
            private readonly Dictionary<GroupBindingModel, int> _refCounts = new Dictionary<GroupBindingModel, int>();

            public CommonParentFinder(Persistence.Version1.ArgumentList arguments)
                : base(arguments)
            { }

            public GroupBindingModel CommonParent
            {
                get
                {
                    return (from kv in _refCounts
                            where kv.Key.Parent != null
                            orderby kv.Value descending
                            select kv.Key).FirstOrDefault();
                }
            }

            private void AddParent(GroupBindingModel parent)
            {
                if (_refCounts.ContainsKey(parent))
                {
                    _refCounts[parent] = _refCounts[parent] + 1;
                }
                else
                {
                    _refCounts[parent] = 1;
                }
            }

            private void TryAddParent(BindingModel bindingModel)
            {
                while (bindingModel != null)
                {
                    var options = bindingModel as GroupBindingModel;

                    if (options != null)
                    {
                        AddParent(options);
                    }

                    bindingModel = bindingModel.Parent;
                }
            }

            protected override void OnNavigateNonValueBindingModel(ValueBindingModel valueBindingModel)
            {
                TryAddParent(valueBindingModel);
            }

            protected override void OnNavigateSingleValueBindingModel(ValueBindingModel valueBindingModel, ValueEntity valueEntity)
            {
                TryAddParent(valueBindingModel);
            }

            protected override void OnNavigateMultiValueBindingModel(ValueBindingModel valueBindingModel, IEnumerable<ValueEntity> values)
            {
                TryAddParent(valueBindingModel);
            }
        }

        private class BindingModelArgumentApplyer : ArgumentBindingModelNavigator
        {
            public BindingModelArgumentApplyer(Persistence.Version1.ArgumentList arguments)
                : base(arguments)
            { }

            public GroupBindingModel CommonParent { get; set; }

            private bool HasCommonParent(BindingModel bindingModel)
            {
                if (CommonParent == null) return true;

                while (bindingModel != null)
                {
                    if (bindingModel == CommonParent)
                    {
                        return true;
                    }

                    bindingModel = bindingModel.Parent;
                }

                return false;
            }

            private void SelectParentPath(BindingModel bindingModel)
            {
                BindingModel lastModel = null;

                while (bindingModel != null)
                {
                    var optionsGroupBinding = bindingModel as OptionsGroupBindingModel;

                    if (optionsGroupBinding != null &&
                        optionsGroupBinding.SelectedOption == null)
                    {
                        optionsGroupBinding.SelectedOption = lastModel;
                    }

                    // next up
                    lastModel = bindingModel;
                    bindingModel = bindingModel.Parent;
                }
            }

            private bool ApplyValue(ValueBindingModel valueBindingModel, ValueEntity valueEntity, Persistence.Version1.Argument arg)
            {
                if (arg.Values.Count > 0)
                {
                    if (valueBindingModel.IsReadOnly)
                    {
                        return (valueEntity.Value == arg.Values[0]);
                    }

                    valueEntity.Value = arg.Values[0];
                    return true;
                }

                return false;
            }

            private bool ApplyValues(ValueBindingModel valueBindingModel, IEnumerable<ValueEntity> values, Persistence.Version1.Argument arg)
            {
                var multiValues = valueBindingModel as MultiValueBindingModel;
                var options = valueBindingModel as OptionsValueBindingModel;

                if (options != null && arg.Values.Count > 0)
                {
                    var selectedOption = (from value in values
                                          where value.ToString() == arg.Values[0]
                                          select value).FirstOrDefault();

                    options.SelectedOption = selectedOption;

                    return true;
                }

                if (multiValues != null)
                {
                    int index = 0;
                    foreach (var svbm in multiValues.Bindings)
                    {
                        svbm.Value.Value = arg.Values[index];

                        index++;
                    }

                    return true;
                }

                return false;
            }

            protected override void OnNavigateNonValueBindingModel(ValueBindingModel valueBindingModel)
            {
                var arg = FindArgument(valueBindingModel.Argument.Name);

                if (arg != null && HasCommonParent(valueBindingModel))
                {
                    var singleValue = valueBindingModel as SingleValueBindingModel;

                    if (singleValue != null)
                    {
                        // checked
                        singleValue.Value.Value = Boolean.TrueString;
                    }

                    SelectParentPath(valueBindingModel);
                }
            }

            protected override void OnNavigateSingleValueBindingModel(ValueBindingModel valueBindingModel, ValueEntity valueEntity)
            {
                var arg = FindArgument(valueBindingModel.Argument.Name);

                if (arg != null && HasCommonParent(valueBindingModel))
                {
                    if (ApplyValue(valueBindingModel, valueEntity, arg))
                    {
                        SelectParentPath(valueBindingModel);
                    }
                }
            }

            protected override void OnNavigateMultiValueBindingModel(ValueBindingModel valueBindingModel, IEnumerable<ValueEntity> values)
            {
                var arg = FindArgument(valueBindingModel.Argument.Name);

                if (arg != null && HasCommonParent(valueBindingModel))
                {
                    if (ApplyValues(valueBindingModel, values, arg))
                    {
                        SelectParentPath(valueBindingModel);
                    }
                }
            }

            protected override void OnNavigateRepeaterValueBindingModel(RepeatingValueBindingModel repeaterBindingModel)
            {
                var arg = FindArgument(repeaterBindingModel.Argument.Name);

                if (arg != null)
                {
                    foreach (var value in arg.Values)
                    {
                        var binding = repeaterBindingModel.Add();

                        // TODO: value?
                        NavigateValue(binding);
                    }
                }
            }
        }
    }
}