using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class RegexValidationRule : ValidationRule
    {
        private Regex regex;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrEmpty(Regex))
            {
                if (this.regex == null)
                {
                    this.regex = new Regex(Regex);
                }

                if (value != null &&
                    value is string)
                {
                    if (!this.regex.IsMatch((string)value))
                    {
                        return new ValidationResult(false, "The field value is invalid (defined by expression).");
                    }
                }
            }

            return new ValidationResult(true, null);
        }

        public string Regex { get; set; }
    }
}