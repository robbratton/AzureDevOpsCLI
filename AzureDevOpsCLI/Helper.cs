using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VSTSTool
{
    public static class Helper
    {
        /// <summary>
        ///     Dump the public instance properties and values
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="input">The instance.</param>
        /// <param name="maskFields">Optional: These values will be replaced with "(null)"</param>
        /// <returns>A string containing a dump of the properties.</returns>
        public static string DumpProperties<T>(T input, IEnumerable<string> maskFields = null) where T : new()
        {
            var propertyValues = new SortedList<string, object>();

            var type = typeof(T);
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var value = type.GetProperty(prop.Name).GetValue(input, null);
                propertyValues.Add(prop.Name, value);
            }

            var output = new StringBuilder();

            foreach (var (propertyKey, propertyValue) in propertyValues)
            {
                var value = propertyValue?.ToString() ?? "(null)";

                // ReSharper disable once PossibleMultipleEnumeration
                if (maskFields?.Contains(propertyKey) == true)
                {
                    output.AppendLine($"{propertyKey}: (masked)");
                }
                else
                {
                    if (propertyValue is IEnumerable<object> items)
                    {
                        // Array, List, etc.
                        output.AppendLine($"{propertyKey} (IEnumerable):");
                        output.AppendLine("  " + string.Join("\n  ", items));
                    }
                    else
                    {
                        // Simple Value
                        output.AppendLine($"{propertyKey}: {value}");
                    }
                }
            }

            return output.ToString();
        }

        public static StringBuilder MakeIndentString(int indent)
        {
            var indentString = new StringBuilder();
            for (var i = 0; i < indent; i++)
            {
                indentString.Append(" ");
            }

            return indentString;
        }
    }
}