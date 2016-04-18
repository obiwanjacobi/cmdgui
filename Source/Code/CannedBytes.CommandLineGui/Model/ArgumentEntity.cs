namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// Model information about the command line argument.
    /// </summary>
    sealed class ArgumentEntity : ModelObject
    {
        /// <summary>
        /// The internal name of the argument.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of values that is used by the argument.
        /// </summary>
        public int ValueCount { get; set; }
    }
}