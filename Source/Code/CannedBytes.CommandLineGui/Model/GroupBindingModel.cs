using System.Collections.Generic;
using System.Linq;

namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// The base class for all Group binding model types.
    /// </summary>
    abstract class GroupBindingModel : BindingModel
    {
        /// <summary>
        /// Default ctor.
        /// </summary>
        public GroupBindingModel()
        {
            Bindings = new List<BindingModel>();
        }

        /// <summary>
        /// A list of all child binding models.
        /// </summary>
        /// <remarks>The list is filled with a mix of <see cref="GroupBindingModel"/> and <see cref="ValueBindingModel"/> instances.</remarks>
        public List<BindingModel> Bindings { get; set; }

        /// <summary>
        /// Returns the <see cref="ValueBindingModel"/>s.
        /// </summary>
        public IEnumerable<ValueBindingModel> ValueBindings
        {
            get
            {
                return from binding in Bindings
                       where (binding is ValueBindingModel)
                       select (ValueBindingModel)binding;
            }
        }

        /// <summary>
        /// Returns the <see cref="GroupBindingModel"/>s.
        /// </summary>
        public IEnumerable<GroupBindingModel> GroupBindings
        {
            get
            {
                return from binding in Bindings
                       where (binding is GroupBindingModel)
                       select (GroupBindingModel)binding;
            }
        }

        public HelpInfo HelpInfo { get; set; }
    }
}