using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Linq;

#warning This namespace contains experimental features that are subject to change outside of semver.
namespace VoidCore.Model.Experimental
{
    // TODO: these are experimental
    /// <summary>
    /// Helpers useful for logging or emailing.
    /// </summary>
    public static class TextHelpers
    {
        private const string DateFormat = "s";

        /// <summary>
        /// Split a string on any newline character.
        /// </summary>
        /// <param name="str">The string to split</param>
        public static string[] SplitOnNewLine(string str)
        {
            return str.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Make an anchor tag string.
        /// </summary>
        /// <param name="caption">The text shown to the user</param>
        /// <param name="urlSegments">A series of url segments to be joined with "/"</param>
        /// <returns></returns>
        public static string Link(string caption, params string[] urlSegments)
        {
            return $"<a href=\"{string.Join("/", urlSegments)}\">{caption}</a>";
        }

        /// <summary>
        /// Print a DateTime to a string.
        /// </summary>
        /// <param name="dateTime">The date to format</param>
        /// <param name="dateFormat">The date format. ISO 8601 by default</param>
        /// <returns></returns>
        public static string Print(DateTime dateTime, string dateFormat = DateFormat)
        {
            dateFormat ??= "s";
            return dateTime.ToString(dateFormat, CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Print an enumerable to a string.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string Print(IEnumerable<string> strings)
        {
            return string.Join(", ", strings);
        }

        /// <summary>
        /// Print an object to a string. Iterates properties to print "Name: Value"
        /// </summary>
        /// <param name="obj">The object to print</param>
        /// <param name="dateFormat">The date format. ISO 8601 by default</param>
        public static IEnumerable<string> PrintObject(object obj, string dateFormat = DateFormat)
        {
            foreach (var property in obj.GetType().GetTypeInfo().GetProperties())
            {
                var value = property.GetValue(obj);

                switch (value)
                {
                    case string str:
                        yield return $"{property.Name}: {str}";
                        break;

                    case DateTime dateTime:
                        yield return $"{property.Name}: {Print(dateTime, dateFormat)}";
                        break;

                    case IEnumerable<DateTime> dateTimes:
                        yield return $"{property.Name}: {Print(dateTimes.Select(d => Print(d, dateFormat)))}";
                        break;

                    case IEnumerable items:
                        List<string> strings = new List<string>();
                        foreach (var item in items)
                        {
                            strings.Add(item.ToString());
                        }

                        yield return $"{property.Name}: {Print(strings)}";
                        break;

                    default:
                        yield return $"{property.Name}: {value}";
                        break;
                }
            }
        }
    }
}
