using System;
using System.Collections.Generic;

namespace CannedBytes.CommandLineGui.Model
{
    /// <summary>
    /// Represents the information for an argument value.
    /// </summary>
    sealed class ValueEntity : ObservableObject, ICloneable
    {
        public ValueEntity()
        { }

        public ValueEntity(string displayValue, string value)
        {
            _displayValue = displayValue;
            _value = value;
        }

        private string _displayValue;

        /// <summary>
        /// The value that is presented to the user.
        /// </summary>
        public string DisplayValue
        {
            get { return _displayValue; }
            set { _displayValue = value; OnPropertyChanged("DisplayValue"); }
        }

        private string _value;

        /// <summary>
        /// The actual value used in the command line.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        /// <summary>
        /// Indicates if the entity actually contains a value.
        /// </summary>
        public bool HasValue
        {
            get
            {
                if (IsSelected.HasValue)
                {
                    return IsSelected.Value;
                }

                return !String.IsNullOrEmpty(Value);
            }
        }

        private bool? _isSelected;

        public bool? IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }

        /// <summary>
        /// Returns the value of the entity.
        /// </summary>
        /// <returns>Returns null when both <see cref="DisplayValue"/> and <see cref="Value"/> are null.</returns>
        public override string ToString()
        {
            if (!String.IsNullOrEmpty(Value))
            {
                return Value;
            }

            if (!String.IsNullOrEmpty(DisplayValue))
            {
                return DisplayValue;
            }

            return String.Empty;
        }

        /// <summary>
        /// A collection of extension properties.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Properties { get; set; }

        private static ValueEntity _empty = new ValueEntity();

        /// <summary>
        /// Retrieves an empty instance. DO NOT CHANGE ITS PROPERTIES!
        /// </summary>
        public static ValueEntity Empty
        {
            get { return _empty; }
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>Never returns null.</returns>
        public ValueEntity Clone()
        {
            var clone = new ValueEntity();

            clone.DisplayValue = this.DisplayValue;
            clone.Value = this.Value;
            clone.IsSelected = this.IsSelected;
            clone.Properties = this.Properties;

            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion ICloneable Members

        /// <summary>
        /// Factory method for creating a new instance.
        /// </summary>
        /// <param name="value">Can be null.</param>
        /// <param name="key">Can be null.</param>
        /// <returns>Never returns null.</returns>
        public static ValueEntity Create(string value, string key)
        {
            var val = new ValueEntity();

            if (String.IsNullOrEmpty(key))
            {
                val.DisplayValue = value;
                val.Value = value;
            }
            else
            {
                val.DisplayValue = value;
                val.Value = key;
            }

            return val;
        }
    }
}