namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// Represents a single argument with a single value.
    /// </summary>
    sealed class SingleValueBindingModel : ValueBindingModel
    {
        /// <summary>
        /// The binding model value.
        /// </summary>
        public ValueEntity Value { get; set; }

        private void CopyTo(SingleValueBindingModel target)
        {
            base.CopyTo(target);

            target.Value = (this.Value == null) ? null : this.Value.Clone();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>Never returns null.</returns>
        public override ValueBindingModel Clone()
        {
            var clone = new SingleValueBindingModel();

            CopyTo(clone);

            return clone;
        }
    }
}