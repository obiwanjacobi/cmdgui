using System.Globalization;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class IntegerMinMaxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                int? integer = null;

                if (value is string)
                {
                    int tempValue;
                    if (!int.TryParse((string)value, out tempValue))
                    {
                        return new ValidationResult(false, "Field value is not an integer.");
                    }

                    integer = tempValue;
                }

                if (value is int)
                {
                    integer = (int)value;
                }

                if (integer != null)
                {
                    if (Min != null && !(integer >= Min))
                    {
                        return new ValidationResult(false, "Field value is too small.");
                    }

                    if (Max != null && !(integer <= Max))
                    {
                        return new ValidationResult(false, "Field value is too big.");
                    }
                }
            }

            return new ValidationResult(true, null);
        }

        public int? Min { get; set; }

        public int? Max { get; set; }

        public static IntegerMinMaxValidationRule Create(string min, string max)
        {
            var rule = new IntegerMinMaxValidationRule();

            int minValue;
            int maxValue;

            if (int.TryParse(min, out minValue))
            {
                rule.Min = minValue;
            }

            if (int.TryParse(max, out maxValue))
            {
                rule.Max = maxValue;
            }

            if (rule.Min == null && rule.Max == null)
            {
                return null;
            }

            return rule;
        }
    }
}