using System.Collections.ObjectModel;

namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// A value binding model that represents an argument that can be specified multiple times.
    /// </summary>
    sealed class RepeatingValueBindingModel : ValueBindingModel
    {
        /// <summary>
        /// Default ctor.
        /// </summary>
        public RepeatingValueBindingModel()
        {
            Bindings = new ObservableCollection<ValueBindingModel>();
        }

        /// <summary>
        /// A binding model that is used as template to create (clone) new instance.
        /// </summary>
        public ValueBindingModel TemplateBindingModel { get; set; }

        /// <summary>
        /// A collection with all the value binding models the user has created.
        /// </summary>
        public ObservableCollection<ValueBindingModel> Bindings { get; set; }

        /// <summary>
        /// Adds a new instance of <see cref="TemplateBindingModel"/> to the <see cref="Bindings"/> collection.
        /// </summary>
        /// <returns>Returns null when <see cref="TemplateBindingModel"/> is null.</returns>
        public ValueBindingModel Add()
        {
            if (TemplateBindingModel == null) return null;

            var vbm = TemplateBindingModel.Clone();

            vbm.Parent = this;
            Bindings.Add(vbm);

            return vbm;
        }
    }
}