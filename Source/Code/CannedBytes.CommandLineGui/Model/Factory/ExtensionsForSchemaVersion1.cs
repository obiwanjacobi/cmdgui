using System;
using System.Collections.Generic;
using System.Linq;

namespace CannedBytes.CommandLineGui.Model.Factory
{
    /// <summary>
    /// Static class containing extension methods for retrieving extension properties.
    /// </summary>
    static class ExtensionsForSchemaVersion1
    {
        /// <summary>
        /// Retrieves the 'selected' attribute.
        /// </summary>
        /// <param name="properties">Must not be null.</param>
        /// <returns>Returns null if not found.</returns>
        public static bool? Selected(this IEnumerable<KeyValuePair<string, string>> properties)
        {
            var selected = (from prop in properties
                            where prop.Key == "selected"
                            select prop.Value).FirstOrDefault();

            bool boolValue;

            if (Boolean.TryParse(selected, out boolValue))
            {
                return boolValue;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the 'helpCmd' attribute.
        /// </summary>
        /// <param name="properties">Must not be null.</param>
        /// <returns>Returns null when not found.</returns>
        public static string HelpCmd(this IEnumerable<KeyValuePair<string, string>> properties)
        {
            var helpCmd = (from kvp in properties
                           where kvp.Key == "helpCmd"
                           select kvp.Value).FirstOrDefault();

            return helpCmd;
        }

        /// <summary>
        /// Retrieves the 'helpUrl' attribute.
        /// </summary>
        /// <param name="properties">Must not be null.</param>
        /// <returns>Returns null when not found.</returns>
        public static string HelpUrl(this IEnumerable<KeyValuePair<string, string>> properties)
        {
            var helpCmd = (from kvp in properties
                           where kvp.Key == "helpUrl"
                           select kvp.Value).FirstOrDefault();

            return helpCmd;
        }

        public static string Mask(this IEnumerable<KeyValuePair<string, string>> properties)
        {
            var mask = (from prop in properties
                        where prop.Key == "mask"
                        select prop.Value).FirstOrDefault();

            return mask;
        }

        public static string MinValue(this IEnumerable<KeyValuePair<string, string>> properties)
        {
            var min = (from prop in properties
                       where prop.Key == "min"
                       select prop.Value).FirstOrDefault();

            return min;
        }

        public static string MaxValue(this IEnumerable<KeyValuePair<string, string>> properties)
        {
            var max = (from prop in properties
                       where prop.Key == "max"
                       select prop.Value).FirstOrDefault();

            return max;
        }
    }
}