using System;
using CannedBytes.CommandLineGui.Schema.Version1;

namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// The base class for all value binding model classes.
    /// </summary>
    abstract class ValueBindingModel : BindingModel, ICloneable
    {
        public ValueBindingModel()
        {
            Control = new BindingControlModel();
        }

        /// <summary>
        /// A reference to the command line argument information.
        /// </summary>
        public ArgumentEntity Argument { get; set; }

        /// <summary>
        /// Control related properties
        /// </summary>
        public new BindingControlModel Control
        {
            get { return (BindingControlModel)base.Control; }
            protected set
            {
                base.Control = value;
            }
        }

        #region ICloneable Members

        /// <summary>
        /// Copies all properties of this instance to the <paramref name="target"/>.
        /// </summary>
        /// <param name="target">If null, no operation is performed.</param>
        protected void CopyTo(ValueBindingModel target)
        {
            if (target != null)
            {
                base.CopyTo(target);

                target.Argument = this.Argument;
                target.Control.Type = this.Control.Type;
            }
        }

        /// <summary>
        /// Used in derived classes to return deep-copies.
        /// </summary>
        /// <returns>Nothing. A <see cref="NotSupportedException"/> is thrown.</returns>
        public virtual ValueBindingModel Clone()
        {
            throw new NotSupportedException();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion ICloneable Members

        //---------------------------------------------------------------------

        public new class BindingControlModel : BindingModel.BindingControlModel
        {
            /// <summary>
            /// An indication of the type of control to use.
            /// </summary>
            public ControlTypes Type { get; set; }
        }
    }
}