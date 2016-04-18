using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using CannedBytes.CommandLineGui.Model.Validation;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui.Model.Factory
{
    /// <summary>
    /// Translates the file structure (version 1) into a view model for the application.
    /// </summary>
    class BindingModelFactoryForSchemaVersion1
    {
        Executable _executable;

        /// <summary>
        /// Creates the binding model for the specified <paramref name="executable"/>.
        /// </summary>
        /// <param name="executable">Must not be null.</param>
        /// <returns>Never returns null.</returns>
        public GroupBindingModel Create(Executable executable)
        {
            if (executable == null) throw new ArgumentNullException("executable");

            _executable = executable;

            var gbm = CreateGroupBindingModel(executable.Gui);

            Fill(gbm, _executable.Gui);

            _executable = null;

            return gbm;
        }

        private GroupBindingModel CreateGroupBindingModel(BindingGroup groupBinding)
        {
            GroupTypes grpCtrl = GroupTypes.Display;

            // determine the type of group binding
            if (groupBinding.Control != null)
            {
                grpCtrl = groupBinding.Control.GroupType;
            }

            GroupBindingModel gbm = null;

            // create the group binding model type
            switch (grpCtrl)
            {
                case GroupTypes.Display:
                    gbm = new DisplayGroupBindingModel();
                    break;
                case GroupTypes.Options:
                    gbm = new OptionsGroupBindingModel();
                    break;
            }

            return gbm;
        }

        private void Fill(GroupBindingModel groupBindingModel, BindingGroup bindingGroup)
        {
            groupBindingModel.Description = bindingGroup.Description;
            groupBindingModel.Name = bindingGroup.Name;
            groupBindingModel.Properties = bindingGroup.PropertyBag;
            groupBindingModel.HelpInfo = CreateHelpInfo(bindingGroup);

            if (bindingGroup.Control != null)
            {
                groupBindingModel.Control.Properties = bindingGroup.Control.PropertyBag;
            }

            for (int index = 0; index < bindingGroup.BindingGroupsAndBindings.Count; index++)
            {
                var bindingSubGroup = bindingGroup.BindingGroupsAndBindings[index] as BindingGroup;
                var binding = bindingGroup.BindingGroupsAndBindings[index] as Binding;

                if (bindingSubGroup != null)
                {
                    var subGroupBindingModel = CreateGroupBindingModel(bindingSubGroup);

                    Fill(subGroupBindingModel, bindingSubGroup);

                    subGroupBindingModel.Parent = groupBindingModel;
                    groupBindingModel.Bindings.Add(subGroupBindingModel);
                }

                if (binding != null)
                {
                    var bindingModel = CreateValueBindingModel(binding);

                    bindingModel.Parent = groupBindingModel;
                    groupBindingModel.Bindings.Add(bindingModel);
                }
            }

            var ogbm = groupBindingModel as OptionsGroupBindingModel;

            if (ogbm != null)
            {
                // preselect the first option
                ogbm.SelectedOption = (from binding in ogbm.GroupBindings
                                       select binding).FirstOrDefault();

                // hide group labels for the first level of group binding children.
                // They are shown as tabs.
                foreach (var group in ogbm.GroupBindings)
                {
                    var displayGroup = group as DisplayGroupBindingModel;

                    if (displayGroup != null)
                    {
                        displayGroup.ShowGroupName = false;
                    }
                }
            }
        }

        private ValueBindingModel CreateValueBindingModel(Binding binding)
        {
            var arg = FindArgument(binding.Argument.Name);

            if (arg == null)
            {
                // schema error
                throw new ArgumentNotFoundException(binding.Name, binding.Argument.Name);
            }

            var argEntity = CreateArgumentEntity(arg);

            if (binding.Control.ControlType == ControlTypes.Options)
            {
                var ovbm = new OptionsValueBindingModel();

                Fill(ovbm, binding, arg, argEntity);

                var valueEntities = new List<ValueEntity>();
                Fill(valueEntities, binding.Values);
                ovbm.Values = valueEntities;

                return ovbm;
            }
            else
            {
                bool repeater = (arg.Multiplicity == Multiplicity.OneOrMore ||
                    arg.Multiplicity == Multiplicity.ZeroOrMore);

                bool multiValue = (argEntity.ValueCount > 1);

                ValueBindingModel template;

                if (multiValue)
                {
                    template = CreateMultiValueBindingModel(binding, arg, argEntity);
                }
                else
                {
                    template = CreateSingleValueBindingModel(binding, arg, argEntity, 0);
                }

                if (repeater)
                {
                    var rvbm = new RepeatingValueBindingModel();
                    rvbm.TemplateBindingModel = template;
                    template.Parent = rvbm;

                    Fill(rvbm, binding, arg, argEntity);

                    if (!template.IsOptional)
                    {
                        Debug.Assert(arg.Multiplicity == Multiplicity.OneOrMore);

                        // must at least have one value
                        rvbm.Add();

                        // others are optional
                        template.IsOptional = true;
                    }

                    return rvbm;
                }

                return template;
            }
        }

        private MultiValueBindingModel CreateMultiValueBindingModel(Binding binding, Argument arg, ArgumentEntity argEntity)
        {
            var mvbm = new MultiValueBindingModel();

            Fill(mvbm, binding, arg, argEntity);
            FillMulti(mvbm, binding, arg, argEntity);

            return mvbm;
        }

        private void FillMulti(MultiValueBindingModel mvbm, Binding binding, Argument arg, ArgumentEntity argEntity)
        {
            for (int index = 0; index < argEntity.ValueCount; index++)
            {
                var singleValueBindingModel = CreateSingleValueBindingModel(binding, arg, argEntity, index);

                if (singleValueBindingModel.Value != null &&
                    !string.IsNullOrEmpty(singleValueBindingModel.Value.DisplayValue))
                {
                    // Nice for presentation of multiple values
                    singleValueBindingModel.Name = singleValueBindingModel.Value.DisplayValue;
                }

                singleValueBindingModel.Parent = mvbm;
                mvbm.Bindings.Add(singleValueBindingModel);
            }
        }

        private SingleValueBindingModel CreateSingleValueBindingModel(Binding binding, Argument arg, ArgumentEntity argEntity, int valueIndex)
        {
            var svbm = new SingleValueBindingModel();

            Fill(svbm, binding, arg, argEntity);

            if (binding.Values.Count > valueIndex)
            {
                svbm.Value = CreateValueEntity(binding.Values[valueIndex]);
            }
            else
            {
                svbm.Value = new ValueEntity();
            }

            if (binding.Control.ControlType == ControlTypes.Check)
            {
                svbm.Value.IsSelected = svbm.Control.Properties.Selected();

                // at least initialize the bool value to false.
                if (!svbm.Value.IsSelected.HasValue)
                {
                    svbm.Value.IsSelected = false;
                }
            }

            return svbm;
        }

        private void Fill(ValueBindingModel valueBindingModel, Binding binding,
            Argument argument, ArgumentEntity argumentEntity)
        {
            Debug.Assert(binding.Argument.Name == argument.Name,
                "Specified argument does not have the same name as specified in the binding.");
            Debug.Assert(binding.Argument.Name == argumentEntity.Name,
                "Specified argumentEntity does not have the same name as specified in the binding.");

            bool optional = (argument.Multiplicity == Multiplicity.ZeroOrOne ||
                argument.Multiplicity == Multiplicity.ZeroOrMore);

            valueBindingModel.Argument = argumentEntity;
            valueBindingModel.Name = binding.Name;
            valueBindingModel.Description = argument.Description;
            valueBindingModel.IsOptional = optional;
            valueBindingModel.IsReadOnly = (binding.Control.ControlType == ControlTypes.Literal);
            valueBindingModel.Control.Type = binding.Control.ControlType;
            valueBindingModel.Control.Properties = binding.Control.PropertyBag;
            valueBindingModel.Properties = binding.PropertyBag;

            CreateValidationRules(valueBindingModel);
        }

        private void Fill(List<ValueEntity> valueEntities, BindingValueList bindingValueList)
        {
            valueEntities.AddRange(
                from value in bindingValueList
                select CreateValueEntity(value));
        }

        private Argument FindArgument(string argumentName)
        {
            var argument = (from arg in _executable.Arguments
                            where arg.Name == argumentName
                            select arg).FirstOrDefault();

            return argument;
        }

        /// <summary>
        /// regex to detect '{0}' in format strings.
        /// </summary>
        private static readonly Regex _valueMultiplicityRegex = new Regex("{[0-9]*", RegexOptions.Compiled);

        private static ArgumentEntity CreateArgumentEntity(Argument argument)
        {
            var argEntity = new ArgumentEntity();

            argEntity.Name = argument.Name;
            argEntity.Properties = argument.PropertyBag;

            var count = 0;
            var matches = _valueMultiplicityRegex.Matches(argument.Format);

            if (matches != null && matches.Count > 0)
            {
                count = matches.Count;
            }

            argEntity.ValueCount = count;

            return argEntity;
        }

        private static ValueEntity CreateValueEntity(BindingValue bindingValue)
        {
            //var val = ValueEntity.Create(bindingValue.Value, bindingValue.Key);
            var val = new ValueEntity(bindingValue.Value, bindingValue.Key);

            val.Properties = bindingValue.PropertyBag;

            val.IsSelected = val.Properties.Selected();

            return val;
        }

        private static HelpInfo CreateHelpInfo(BindingGroup bindingGroup)
        {
            var helpCmd = bindingGroup.PropertyBag.HelpCmd();
            var helpUrl = bindingGroup.PropertyBag.HelpUrl();

            return new HelpInfo(helpCmd, helpUrl);
        }

        private static void CreateValidationRules(ValueBindingModel valueBindingModel)
        {
            // validation rules
            var rules = new List<ValidationRule>();

            if (!valueBindingModel.IsOptional)
            {
                switch (valueBindingModel.Control.Type)
                {
                    case ControlTypes.Options:
                        rules.Add(new OptionSelectedValidationRule());
                        break;
                    case ControlTypes.Folder:
                    case ControlTypes.InputFile:
                    case ControlTypes.Integer:
                    case ControlTypes.Multiline:
                    case ControlTypes.Number:
                    case ControlTypes.OutputFile:
                    case ControlTypes.Text:
                    case ControlTypes.Uri:
                        rules.Add(new NotNullOrEmptyValidationRule());
                        break;
                }
            }

            // these could also be hardcoded in the xaml of the control itself
            // but we want to keep flexibility for when to activate them.
            switch (valueBindingModel.Control.Type)
            {
                case ControlTypes.Folder:
                    rules.Add(new FolderExistsValidationRule());
                    break;
                case ControlTypes.InputFile:
                    rules.Add(new FileExistsValiadtionRule());
                    break;
                case ControlTypes.Uri:
                    rules.Add(new UriValidationRule());
                    break;
            }

            var mask = valueBindingModel.Control.Properties.Mask();

            if (!string.IsNullOrWhiteSpace(mask))
            {
                switch (valueBindingModel.Control.Type)
                {
                    case ControlTypes.Folder:
                    case ControlTypes.InputFile:
                    case ControlTypes.OutputFile:
                        // no-op
                        break;

                    case ControlTypes.Text:
                        var rule = new RegexValidationRule();
                        rule.Regex = mask;
                        rules.Add(rule);
                        break;

                    default:
                        Trace.WriteLine(string.Format(
                            "No validation rule was applied to '{0}' for the mask attribute '{1}'.",
                            valueBindingModel.Name, mask), "ValidationRules");
                        break;
                }
            }

            var min = valueBindingModel.Control.Properties.MinValue();
            var max = valueBindingModel.Control.Properties.MaxValue();

            if (!string.IsNullOrWhiteSpace(min) || !string.IsNullOrWhiteSpace(max))
            {
                switch (valueBindingModel.Control.Type)
                {
                    case ControlTypes.Integer:
                        var intRule = IntegerMinMaxValidationRule.Create(min, max);
                        if (intRule != null)
                        {
                            rules.Add(intRule);
                        }
                        break;

                    case ControlTypes.Number:
                        var nmbRule = NumberMinMacValidationRule.Create(min, max);
                        if (nmbRule != null)
                        {
                            rules.Add(nmbRule);
                        }
                        break;

                    case ControlTypes.Folder:
                    case ControlTypes.InputFile:
                    case ControlTypes.Multiline:
                    case ControlTypes.OutputFile:
                    case ControlTypes.Text:
                    case ControlTypes.Uri:
                        var lngRule = StringLengthValidationRule.Create(min, max);
                        if (lngRule != null)
                        {
                            rules.Add(lngRule);
                        }
                        break;

                    default:
                        Trace.WriteLine(string.Format(
                            "No validation rule was applied to '{0}' for the min ('{1}') and max ('{2}') attributes.",
                            valueBindingModel.Name, min, max), "ValidationRules");
                        break;
                }
            }

            if (rules.Count > 0)
            {
                valueBindingModel.Control.ValidationRules = rules;
            }
        }
    }
}