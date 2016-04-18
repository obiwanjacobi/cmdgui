using System;
using System.Globalization;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class NotNullOrEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isValid = false;

            if (value != null)
            {
                var str = value as string;

                if (str != null &&
                    !string.IsNullOrWhiteSpace(str))
                {
                    isValid = true;
                }

                if (value is Guid)
                {
                    var guid = (Guid)value;

                    isValid = guid != Guid.Empty;
                }
            }

            return new ValidationResult(isValid, "Field cannot have an empty value.");
        }
    }
}