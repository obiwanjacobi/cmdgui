namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// Binding model information about displaying a group of binding models.
    /// </summary>
    sealed class DisplayGroupBindingModel : GroupBindingModel
    {
        internal DisplayGroupBindingModel()
        {
            ShowGroupName = true;
        }

        /// <summary>
        /// Indicates to the control wether to show the group name or not.
        /// </summary>
        public bool ShowGroupName { get; set; }
    }
}