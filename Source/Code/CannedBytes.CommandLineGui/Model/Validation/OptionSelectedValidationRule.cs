using System.Globalization;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class OptionSelectedValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(value != null, "No option has been selected.");
        }
    }
}