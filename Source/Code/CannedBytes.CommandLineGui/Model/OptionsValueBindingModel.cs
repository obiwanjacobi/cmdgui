using System.Collections.Generic;

namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// The value binding model that represents an option list of values.
    /// </summary>
    sealed class OptionsValueBindingModel : ValueBindingModel
    {
        /// <summary>
        /// When non-null holds the selected value.
        /// </summary>
        public ValueEntity SelectedOption { get; set; }

        /// <summary>
        /// The values that can be selected for this value binding model.
        /// </summary>
        public IEnumerable<ValueEntity> Values { get; set; }
    }
}