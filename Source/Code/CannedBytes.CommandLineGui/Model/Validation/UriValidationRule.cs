using System;
using System.Globalization;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class UriValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                if (value is string)
                {
                    Uri uri;
                    if (!Uri.TryCreate((string)value, UriKind.Absolute, out uri))
                    {
                        return new ValidationResult(false, "Field value is not a valid URI.");
                    }
                }
            }

            return new ValidationResult(true, null);
        }
    }
}