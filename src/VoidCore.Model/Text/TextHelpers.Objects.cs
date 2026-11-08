using System.Collections;
using System.Globalization;
using System.Reflection;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Text;

/// <summary>
/// Helpers useful for logging or emailing.
/// </summary>
public static partial class TextHelpers
{
    private const string DefaultDateFormat = "s";

    /// <summary>
    /// Print a DateTime to a string.
    /// </summary>
    /// <param name="dateTime">The date to format</param>
    /// <param name="dateFormat">The date format. ISO 8601 by default</param>
    public static string Print(DateTime dateTime, string dateFormat = DefaultDateFormat)
    {
        return dateTime.ToString(dateFormat, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Print an enumerable to a string.
    /// </summary>
    /// <param name="strings"></param>
    public static string Print(IEnumerable<string> strings)
    {
        return string.Join(", ", strings);
    }

    /// <summary>
    /// Print an object to a series of strings. Iterates properties to print "Name: Value"
    /// </summary>
    /// <param name="obj">The object to print</param>
    /// <param name="dateFormat">The date format. ISO 8601 by default</param>
    /// <exception cref="ArgumentNullException">The object is null</exception>
    public static IEnumerable<string> PrintObject(object obj, string dateFormat = DefaultDateFormat)
    {
        obj.EnsureNotNull();

        return PrintProperties(obj, dateFormat);
    }

    private static IEnumerable<string> PrintProperties(object obj, string dateFormat)
    {
        // Indexers need arguments, so they can't be printed.
        var properties = obj.GetType().GetTypeInfo().GetProperties()
            .Where(property => property.GetIndexParameters().Length == 0);

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

                case DateTimeOffset dateTimeOffset:
                    yield return $"{property.Name}: {dateTimeOffset.ToString(dateFormat, CultureInfo.InvariantCulture)}";
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
                    yield return $"{property.Name}: {Convert.ToString(value, CultureInfo.InvariantCulture)}";
                    break;
            }
        }
    }
}
