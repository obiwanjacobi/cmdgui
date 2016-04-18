using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.Model.Validation
{
    class FolderExistsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                var str = value as string;

                if (str != null &&
                    !string.IsNullOrWhiteSpace(str) &&
                    !Directory.Exists(str))
                {
                    return new ValidationResult(false, "The directory does not exist.");
                }
            }

            return new ValidationResult(true, null);
        }
    }
}