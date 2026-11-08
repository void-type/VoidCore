using System.Globalization;
using VoidCore.Model.Text;
using Xunit;

namespace VoidCore.Test.Model.Text;

public class TextHelpersHardeningTests
{
    [Theory]
    [InlineData("Straße Æther Øl Łódź", "strasse-aether-ol-lodz")]
    [InlineData("a - b -", "a-b")]
    [InlineData("-hello- ", "hello")]
    [InlineData("--", "")]
    public void Slugify_transliterates_letters_without_accents_and_trims_hyphens(string input, string expected)
    {
        Assert.Equal(expected, input.Slugify());
    }

    [Fact]
    public void Slugify_negative_max_length_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "hello world".Slugify(-1));
    }

    [Fact]
    public void Slugify_zero_max_length_is_empty()
    {
        Assert.Equal(string.Empty, "hello world".Slugify(0));
        Assert.Equal(string.Empty, "hello world".Slugify(0, true));
    }

    [Fact]
    public void Slugify_split_on_word_cuts_a_single_long_word_instead_of_returning_nothing()
    {
        Assert.Equal("supercal", "supercalifragilistic expialidocious".Slugify(8, true));
    }

    [Fact]
    public void PrintObject_throws_immediately_for_null_instead_of_when_enumerated()
    {
        Assert.Throws<ArgumentNullException>(() => TextHelpers.PrintObject(null!));
    }

    [Fact]
    public void PrintObject_skips_indexers()
    {
        var lines = TextHelpers.PrintObject(new WithIndexer { Name = "x" }).ToList();

        Assert.Equal(["Name: x"], lines);
    }

    [Fact]
    public void PrintObject_formats_numbers_and_offsets_with_the_invariant_culture()
    {
        var original = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            var lines = TextHelpers.PrintObject(new WithValues(), "yyyy-MM-dd").ToList();

            Assert.Contains("Amount: 1234.5", lines);
            Assert.Contains("Offset: 2001-02-12", lines);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    private sealed class WithIndexer
    {
        public string Name { get; set; } = string.Empty;

        public int this[int index] => index;
    }

    private sealed class WithValues
    {
        public decimal Amount { get; set; } = 1234.5m;
        public DateTimeOffset Offset { get; set; } = new(2001, 2, 12, 0, 0, 0, TimeSpan.Zero);
    }
}
