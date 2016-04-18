using System.Collections.Generic;
using System.Linq;

namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// A binding model for arguments that require multiple values.
    /// </summary>
    sealed class MultiValueBindingModel : ValueBindingModel
    {
        /// <summary>
        /// Default ctor.
        /// </summary>
        public MultiValueBindingModel()
        {
            Bindings = new List<SingleValueBindingModel>();
        }

        /// <summary>
        /// A collection of <see cref="SingleValueBindingModel"/> instances, one for each value.
        /// </summary>
        public List<SingleValueBindingModel> Bindings { get; private set; }

        private void CopyTo(MultiValueBindingModel target)
        {
            base.CopyTo(target);

            target.Bindings.AddRange(
                from binding in this.Bindings
                select (SingleValueBindingModel)binding.Clone());
        }

        /// <summary>
        /// Returns a deep copy of this instance.
        /// </summary>
        /// <returns></returns>
        public override ValueBindingModel Clone()
        {
            var clone = new MultiValueBindingModel();

            CopyTo(clone);

            return clone;
        }
    }
}