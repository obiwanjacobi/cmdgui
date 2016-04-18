namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// A group binding model that represents an option list.
    /// </summary>
    sealed class OptionsGroupBindingModel : GroupBindingModel
    {
        internal OptionsGroupBindingModel()
        { }

        /// <summary>
        /// When non-null it hold the selected binding model.
        /// </summary>
        public BindingModel SelectedOption { get; set; }

        /// <summary>
        /// For UI only.
        /// </summary>
        public bool IsExpanded { get; set; }
    }
}