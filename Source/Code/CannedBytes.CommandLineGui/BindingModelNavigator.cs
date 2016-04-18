using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CannedBytes.CommandLineGui.Model;

namespace CannedBytes.CommandLineGui
{
    abstract class BindingModelNavigator
    {
        /// <summary>
        /// When true SelectedOptions are ignored and the complete structure is navigated.
        /// </summary>
        public bool NavigateAll { get; set; }

        protected virtual void OnNavigateNonValueBindingModel(ValueBindingModel valueBindingModel)
        {
        }

        protected virtual void OnNavigateSingleValueBindingModel(ValueBindingModel valueBindingModel, ValueEntity valueEntity)
        {
        }

        protected virtual void OnNavigateMultiValueBindingModel(ValueBindingModel valueBindingModel, IEnumerable<ValueEntity> values)
        {
        }

        protected virtual void OnNavigateRepeaterValueBindingModel(RepeatingValueBindingModel repeaterBindingModel)
        {
            foreach (var vbm in repeaterBindingModel.Bindings)
            {
                NavigateValue(vbm);
            }
        }

        public virtual void Navigate(BindingModel bindingModel)
        {
            var groupBindingModel = bindingModel as GroupBindingModel;
            var valueBindingModel = bindingModel as ValueBindingModel;

            if (groupBindingModel != null)
            {
                NavigateGroup(groupBindingModel);
            }

            if (valueBindingModel != null)
            {
                NavigateValue(valueBindingModel);
            }
        }

        protected virtual void NavigateGroup(GroupBindingModel bindingModel)
        {
            var optionsGroupBindingModel = bindingModel as OptionsGroupBindingModel;

            // special case for option group
            if (optionsGroupBindingModel != null && !NavigateAll)
            {
                bool hasSubGroups = bindingModel.GroupBindings.Any();

                // is it a GroupOptoinsContainerControl i.e. Tabs?
                if (hasSubGroups)
                {
                    foreach (var valueBindingModel in bindingModel.ValueBindings)
                    {
                        NavigateValue(valueBindingModel);
                    }
                }

                Navigate(optionsGroupBindingModel.SelectedOption);
            }
            else
            {
                foreach (var binding in bindingModel.Bindings)
                {
                    Navigate(binding);
                }
            }
        }

        protected virtual void NavigateValue(ValueBindingModel bindingModel)
        {
            var singleValueBindingModel = bindingModel as SingleValueBindingModel;
            var multiValueBindingModel = bindingModel as MultiValueBindingModel;
            var optionsValueBindingModel = bindingModel as OptionsValueBindingModel;
            var repeaterBindingModel = bindingModel as RepeatingValueBindingModel;

#if DEBUG
            if (singleValueBindingModel == null &&
                multiValueBindingModel == null &&
                optionsValueBindingModel == null &&
                repeaterBindingModel == null)
            {
                Trace.WriteLine("Warning: Skipping binding model: " + bindingModel.Name);
            }
#endif

            if (singleValueBindingModel != null &&
                (singleValueBindingModel.Value.HasValue || NavigateAll))
            {
                switch (bindingModel.Argument.ValueCount)
                {
                    case 0:
                        OnNavigateNonValueBindingModel(singleValueBindingModel);
                        break;
                    case 1:
                        OnNavigateSingleValueBindingModel(singleValueBindingModel, singleValueBindingModel.Value);
                        break;
                }
            }

            if (optionsValueBindingModel != null)
            {
                if (NavigateAll)
                {
                    OnNavigateMultiValueBindingModel(optionsValueBindingModel, optionsValueBindingModel.Values);
                }
                else if (optionsValueBindingModel.SelectedOption != null &&
                         optionsValueBindingModel.SelectedOption.HasValue)
                {
                    OnNavigateSingleValueBindingModel(optionsValueBindingModel, optionsValueBindingModel.SelectedOption);
                }
            }

            if (multiValueBindingModel != null)
            {
                var values = from binding in multiValueBindingModel.Bindings
                             where binding.Value.HasValue == true
                             select binding.Value;

                OnNavigateMultiValueBindingModel(multiValueBindingModel, values);
            }

            if (repeaterBindingModel != null)
            {
                OnNavigateRepeaterValueBindingModel(repeaterBindingModel);
            }
        }
    }
}