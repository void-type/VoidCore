using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Text;

/// <summary>
/// Helpers useful for logging or emailing.
/// </summary>
public static partial class TextHelpers
{
    private const string DateFormat = "s";

    /// <summary>
    /// Print a DateTime to a string.
    /// </summary>
    /// <param name="dateTime">The date to format</param>
    /// <param name="dateFormat">The date format. ISO 8601 by default</param>
    /// <returns></returns>
    public static string Print(DateTime dateTime, string dateFormat = DateFormat)
    {
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
    /// Print an object to a series of strings. Iterates properties to print "Name: Value"
    /// </summary>
    /// <param name="obj">The object to print</param>
    /// <param name="dateFormat">The date format. ISO 8601 by default</param>
    public static IEnumerable<string> PrintObject(object obj, string dateFormat = DateFormat)
    {
        var properties = obj
            .EnsureNotNull()
            .GetType().GetTypeInfo().GetProperties();

        foreach (var property in properties)
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
                    var strings = new List<string>();
                    foreach (var item in items)
                    {
                        strings.Add($"{item}");
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
