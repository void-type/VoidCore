using BenchmarkDotNet.Attributes;
using System.Linq;
using VoidCore.Model.Text;

namespace VoidCore.Benchmark;

// Testing various types of loops.
[MemoryDiagnoser]
public class TextHelpersBenchmarks
{
    [Benchmark]
    public void GetFirstNotEmptyOrDefault_Bench()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two",
        };

        TextHelpers.GetFirstNotEmptyOrDefault(values);
    }

    public static string GetFirstNotEmptyOrDefault2(params string?[] values)
    {
        return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
    }

    [Benchmark]
    public void GetFirstNotEmptyOrDefault2_Bench()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two",
        };

        GetFirstNotEmptyOrDefault2(values);
    }

    public static string GetFirstNotEmptyOrDefault3(params string?[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(values[i]))
            {
                return values[i];
            }
        }

        return string.Empty;
    }

    [Benchmark]
    public void GetFirstNotEmptyOrDefault3_Bench()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two",
        };

        GetFirstNotEmptyOrDefault3(values);
    }

    public static string GetFirstNotEmptyOrDefault4(params string?[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            var value = values[i];

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return string.Empty;
    }

    [Benchmark]
    public void GetFirstNotEmptyOrDefault4_Bench()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two",
        };

        GetFirstNotEmptyOrDefault3(values);
    }
}
