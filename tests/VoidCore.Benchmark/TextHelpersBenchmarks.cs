using BenchmarkDotNet.Attributes;
using System.Linq;
using VoidCore.Model.Text;

namespace VoidCore.Benchmark;

// Testing various types of loops.
[MemoryDiagnoser]
public class TextHelpersBenchmarks
{
    [Benchmark]
    public void FirstNotNullOrWhiteSpace_Bench()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two",
        };

        TextHelpers.FirstNotNullOrWhiteSpace(values);
    }

    public static string FirstNotNullOrWhiteSpace2(params string?[] values)
    {
        return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty;
    }

    [Benchmark]
    public void FirstNotNullOrWhiteSpace2_Bench()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two",
        };

        FirstNotNullOrWhiteSpace2(values);
    }

    public static string FirstNotNullOrWhiteSpace3(params string?[] values)
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
    public void FirstNotNullOrWhiteSpace3_Bench()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two",
        };

        FirstNotNullOrWhiteSpace3(values);
    }

    public static string FirstNotNullOrWhiteSpace4(params string?[] values)
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
    public void FirstNotNullOrWhiteSpace4_Bench()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two",
        };

        FirstNotNullOrWhiteSpace3(values);
    }
}
