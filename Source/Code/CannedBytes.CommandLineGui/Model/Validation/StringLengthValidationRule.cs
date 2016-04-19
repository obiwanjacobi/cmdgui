using System;
using System.Globalization;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class StringLengthValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                if (value is string)
                {
                    var str = (string)value;

                    if (MinLength != null && !(str.Length >= MinLength))
                    {
                        return new ValidationResult(false, "Field value too short.");
                    }

                    if (MaxLength != null && !(str.Length <= MaxLength))
                    {
                        return new ValidationResult(false, "Field value too long.");
                    }
                }
            }

            return new ValidationResult(true, null);
        }

        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }

        public static StringLengthValidationRule Create(string min, string max)
        {
            var rule = new StringLengthValidationRule();

            int minValue;
            int maxValue;

            if (Int32.TryParse(min, out minValue))
            {
                rule.MinLength = minValue;
            }

            if (Int32.TryParse(max, out maxValue))
            {
                rule.MaxLength = maxValue;
            }

            if (rule.MinLength == null && rule.MaxLength == null)
            {
                return null;
            }

            return rule;
        }
    }
}