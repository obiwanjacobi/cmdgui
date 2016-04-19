using System;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class NumberMinMaxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value != null)
            {
                double? dbl = null;

                if (value is string)
                {
                    double tempValue;
                    if (!Double.TryParse((string)value, out tempValue))
                    {
                        return new ValidationResult(false, "Field value is not an integer.");
                    }

                    dbl = tempValue;
                }

                if (value is double)
                {
                    dbl = (double)value;
                }

                if (dbl != null)
                {
                    if (Min != null && !(dbl >= Min))
                    {
                        return new ValidationResult(false, "Field value is too small.");
                    }

                    if (Max != null && !(dbl <= Max))
                    {
                        return new ValidationResult(false, "Field value is too big.");
                    }
                }
            }

            return new ValidationResult(true, null);
        }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public static NumberMinMaxValidationRule Create(string min, string max)
        {
            var rule = new NumberMinMaxValidationRule();

            double minValue;
            double maxValue;

            if (Double.TryParse(min, out minValue))
            {
                rule.Min = minValue;
            }

            if (Double.TryParse(max, out maxValue))
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