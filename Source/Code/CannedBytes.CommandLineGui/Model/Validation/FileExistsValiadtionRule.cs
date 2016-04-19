using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class FileExistsValiadtionRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                var str = value as string;

                if (str != null &&
                    !String.IsNullOrWhiteSpace(str) &&
                    !File.Exists(str))
                {
                    return new ValidationResult(false, "The file does not exist.");
                }
            }

            return new ValidationResult(true, null);
        }
    }
}