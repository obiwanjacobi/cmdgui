using System.Collections.Generic;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// The base class for all binding model classes.
    /// </summary>
    abstract class BindingModel : ModelObject
    {
        public BindingModel()
        {
            Control = new BindingControlModel();
        }

        /// <summary>
        /// The name of the binding model object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A helpful description on the purpose or function of the binding model object.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An indication if this binding model object is optional or mandatory.
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        /// An indication if this binding model object is read-only or writable.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// A reference up to the parent of this binding model object.
        /// </summary>
        public BindingModel Parent { get; set; }

        /// <summary>
        /// Control related properties
        /// </summary>
        public BindingControlModel Control { get; protected set; }

        /// <summary>
        /// Copies all properties of this instance to the <paramref name="target"/>.
        /// </summary>
        /// <param name="target">If null, no operation is performed.</param>
        protected void CopyTo(BindingModel target)
        {
            if (target != null)
            {
                target.Description = this.Description;
                target.Name = this.Name;
                target.IsOptional = this.IsOptional;
                target.Parent = this.Parent;
                target.Properties = this.Properties;
                target.Control.Properties = this.Control.Properties;
                target.Control.ValidationRules = this.Control.ValidationRules;
            }
        }

        //---------------------------------------------------------------------

        public class BindingControlModel
        {
            /// <summary>
            /// Extension attributes for the control
            /// </summary>
            public IEnumerable<KeyValuePair<string, string>> Properties { get; set; }

            /// <summary>
            /// Validation rules to be applied to the control's main edit control.
            /// </summary>
            public IEnumerable<ValidationRule> ValidationRules { get; set; }
        }
    }
}