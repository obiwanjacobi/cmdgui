using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// Converts a bool to a Visibility valule.
    /// </summary>
    /// <remarks>
    /// Support a bool parameter. When true the logic is inversed.
    /// </remarks>
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else
            {
                if (value is bool?)
                {
                    bool? flag2 = (bool?)value;
                    flag = (flag2.HasValue && flag2.Value);
                }
            }

            if (Parse(parameter))
            {
                flag = !flag;
            }

            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                bool flag = (Visibility)value == Visibility.Visible;

                if (Parse(parameter))
                {
                    flag = !flag;
                }

                return flag;
            }

            return false;
        }

        private bool Parse(object parameter)
        {
            if (parameter != null &&
                parameter is string)
            {
                bool value;

                if (bool.TryParse((string)parameter, out value))
                {
                    return value;
                }
            }

            return false;
        }
    }
}