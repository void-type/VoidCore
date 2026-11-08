using VoidCore.Model.Text;
using Xunit;

namespace VoidCore.Test.Model.Text;

public class TextHelpersTests
{
    [Fact]
    public void Can_make_lines_from_object_properties_with_dates_and_arrays()
    {
        var strings = TextHelpers.PrintObject(new TestObject(), "MM/dd/yyyy h:mm tt");

        Assert.Equal(
            [
                "MyString: Hello World",
                "MyInteger: 201",
                "MyStrings: 5, 6, 7, 8",
                "MyIntegers: 1, 2, 3, 4",
                "MyDateTime: 12/01/2018 9:09 AM",
                "MyDate: 09/10/2008 12:00 AM",
                "MyDateTimes: 09/10/2008 12:00 AM, 12/01/2018 9:09 AM, 09/10/2008 12:00 AM",
            ], strings);
    }

    [Fact]
    public void Can_format_dates_to_ISO8601_by_default()
    {
        var strings = TextHelpers.PrintObject(new TestObject());

        Assert.Equal(
            [
                "MyString: Hello World",
                "MyInteger: 201",
                "MyStrings: 5, 6, 7, 8",
                "MyIntegers: 1, 2, 3, 4",
                "MyDateTime: 2018-12-01T09:09:08",
                "MyDate: 2008-09-10T00:00:00",
                "MyDateTimes: 2008-09-10T00:00:00, 2018-12-01T09:09:08, 2008-09-10T00:00:00",
            ], strings);
    }

    [Fact]
    public void Can_make_web_links()
    {
        var link = TextHelpers.Link("Click me", "https://www.contoso.com", "recipes", "1");

        Assert.Equal("<a href=\"https://www.contoso.com/recipes/1\">Click me</a>", link);
    }

    [Fact]
    public void Link_encodes_the_caption_and_url()
    {
        var link = TextHelpers.Link("<script>alert(1)</script>", "https://www.contoso.com/a\"onclick=\"x", "b&c");

        Assert.Equal(
            "<a href=\"https://www.contoso.com/a&quot;onclick=&quot;x/b&amp;c\">&lt;script&gt;alert(1)&lt;/script&gt;</a>",
            link);
    }

    [Theory]
    [InlineData("javascript:alert(1)")]
    [InlineData("JavaScript:alert(1)")]
    [InlineData("java\tscript:alert(1)")]
    [InlineData("  javascript:alert(1)")]
    [InlineData("data:text/html,<script>alert(1)</script>")]
    public void Link_rejects_unsafe_schemes(string url)
    {
        Assert.Throws<ArgumentException>(() => TextHelpers.Link("x", url));
    }

    [Theory]
    [InlineData("https://www.contoso.com", "https://www.contoso.com")]
    [InlineData("mailto:me@contoso.com", "mailto:me@contoso.com")]
    [InlineData("/recipes/1", "/recipes/1")]
    [InlineData("recipes/1:2", "recipes/1:2")]
    [InlineData("/search?q=a:b", "/search?q=a:b")]
    public void Link_allows_safe_and_relative_urls(string url, string expectedHref)
    {
        Assert.Equal($"<a href=\"{expectedHref}\">x</a>", TextHelpers.Link("x", url));
    }

    [Fact]
    public void Can_split_text_on_any_new_line()
    {
        var strings = TextHelpers.SplitOnNewLine("Click me\nhttps://www.contoso.com\r\nrecipes\\1");

        Assert.Equal<string[]>(["Click me", "https://www.contoso.com", "recipes\\1"], strings);
    }

    [Fact]
    public void String_extensions()
    {
        Assert.True("".IsNullOrEmpty());
        Assert.True(((string)null!).IsNullOrEmpty());
        Assert.False("\t ".IsNullOrEmpty());
        Assert.False("t".IsNullOrEmpty());

        Assert.True("".IsNullOrWhiteSpace());
        Assert.True(((string)null!).IsNullOrWhiteSpace());
        Assert.True("\t ".IsNullOrWhiteSpace());
        Assert.False("t".IsNullOrWhiteSpace());

        Assert.True("t".EqualsIgnoreCase("T"));
        Assert.True("t".EqualsIgnoreCase("t"));
        Assert.False("ts".EqualsIgnoreCase("t"));
    }

    [Fact]
    public void String_default_extensions()
    {
        Assert.Equal("default", "".DefaultIfNullOrEmpty("default"));
        Assert.Equal("default", ((string)null!).DefaultIfNullOrEmpty("default"));
        Assert.Equal("\t ", "\t ".DefaultIfNullOrEmpty("default"));
        Assert.Equal("t", "t".DefaultIfNullOrEmpty("default"));

        Assert.Equal("default", "".DefaultIfNullOrWhiteSpace("default"));
        Assert.Equal("default", ((string)null!).DefaultIfNullOrWhiteSpace("default"));
        Assert.Equal("default", "\t ".DefaultIfNullOrWhiteSpace("default"));
        Assert.Equal("t", "t".DefaultIfNullOrWhiteSpace("default"));
    }

    [Fact]
    public void Print_date_uses_ISO8601_format_by_default()
    {
        var myDate = new DateTime(2008, 9, 10);

        var myString = TextHelpers.Print(myDate);

        Assert.Equal("2008-09-10T00:00:00", myString);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Slugify_returns_empty_string_when_input_null_or_whitespace(string? input)
    {
        var actual = input.Slugify();

        Assert.Equal(string.Empty, actual);
    }

    [Fact]
    public void Slugify_removes_extra_spaces_accents_and_special_chars()
    {
        var actual = "The - quick; Brown < Fo>X\nJúmp,ed over the lÅzÿ Dòg 大2".Slugify();
        var expected = "the-quick-brown-fox-jumped-over-the-lazy-dog-2";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Slugify_truncates_at_character_count()
    {
        var actual = "The   quick; Brown < Fo>X Júmp,ed over the lÅzÿ Dòg 大".Slugify(28);
        var expected = "the-quick-brown-fox-jumped-o";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Slugify_truncates_at_character_count_trims_trailing_space()
    {
        var actual = "The   quick; Brown < Fo>X Júmp,ed over the lÅzÿ Dòg 大".Slugify(27);
        var expected = "the-quick-brown-fox-jumped";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Slugify_truncates_at_character_count_at_whole_word()
    {
        var actual = "The   quick; Brown < Fo>X Júmp,ed over the lÅzÿ Dòg 大".Slugify(26);
        var expected = "the-quick-brown-fox-jumped";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Slugify_truncates_at_word_removes_incomplete_word()
    {
        var actual = "The   quick; Brown < Fo>X Júmp,ed over the lÅzÿ Dòg 大".Slugify(29, true);
        var expected = "the-quick-brown-fox-jumped";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Slugify_truncates_at_word_trims_trailing_spaces()
    {
        var actual = "The   quick; Brown < Fo>X Júmp,ed over the lÅzÿ Dòg 大".Slugify(27, true);
        var expected = "the-quick-brown-fox-jumped";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Slugify_truncates_at_word_leaves_complete_word()
    {
        var actual = "The   quick; Brown < Fo>X Júmp,ed over the lÅzÿ Dòg 大".Slugify(26, true);
        var expected = "the-quick-brown-fox-jumped";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetSafeFileName_strips_illegal_chars()
    {
        var actual = @"C:\../..\my-file.txt".GetSafeFileName();

        Assert.Equal("C______my-file.txt", actual);
    }

    [Theory]
    [InlineData("report.pdf", "report.pdf")]
    [InlineData("a:b*c?.txt", "a_b_c_.txt")]
    [InlineData("name.", "name")]
    [InlineData("name . ", "name")]
    [InlineData("a....b", "a__b")]
    [InlineData("", "_")]
    [InlineData(".", "_")]
    [InlineData("..", "_")]
    [InlineData("   ", "_")]
    [InlineData("CON", "_CON")]
    [InlineData("nul.txt", "_nul.txt")]
    [InlineData("Com1.log", "_Com1.log")]
    [InlineData("console.txt", "console.txt")]
    [InlineData("a\r\nb", "a__b")]
    public void GetSafeFileName_makes_names_safe_on_every_platform(string input, string expected)
    {
        Assert.Equal(expected, input.GetSafeFileName());
    }

    [Fact]
    public void GetSafeFileName_leaves_no_dot_dot_after_replacing_with_a_dot()
    {
        Assert.DoesNotContain("..", "...".GetSafeFileName("."), StringComparison.Ordinal);
        Assert.DoesNotContain("..", "a.....b".GetSafeFileName("."), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("..")]
    [InlineData("/")]
    [InlineData("a:")]
    public void GetSafeFileName_rejects_replacements_that_could_make_it_unsafe(string replacement)
    {
        Assert.Throws<ArgumentException>(() => "file.txt".GetSafeFileName(replacement));
    }

    [Fact]
    public void GetSafeFilePath_strips_illegal_chars()
    {
        var actual = @"C:\../..\my-file.txt".GetSafeFilePath();
        var expected = @"C:\_/_\my-file.txt";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetSafeFilePath_leaves_no_dot_dot_after_replacing_with_a_dot()
    {
        Assert.DoesNotContain("..", "a/...../b".GetSafeFilePath("."), StringComparison.Ordinal);
    }

    [Fact]
    public void FirstNotNullOrWhiteSpace_returns_first_string_thats_not_null_or_whitespace()
    {
        var values = new[]
        {
            null,
            "",
            " ",
            "one",
            "two"
        };

        Assert.Equal("one", TextHelpers.FirstNotNullOrWhiteSpace(values));
    }

    [Fact]
    public void FirstNotNullOrWhiteSpace_returns_empty_string_if_all_are_null_or_whitespace()
    {
        var values = new[]
        {
            "",
            " ",
            null,
        };

        Assert.Equal(string.Empty, TextHelpers.FirstNotNullOrWhiteSpace(values));
    }

    [Fact]
    public void FirstNotNullOrWhiteSpace_returns_empty_string_if_no_values()
    {
        var values = Array.Empty<string>();

        Assert.Equal(string.Empty, TextHelpers.FirstNotNullOrWhiteSpace(values));
    }

    public class TestObject
    {
        public string MyString { get; set; } = "Hello World";
        public int MyInteger { get; set; } = 201;
        public string[] MyStrings { get; set; } = ["5", "6", "7", "8"];
        public int[] MyIntegers { get; set; } = [1, 2, 3, 4];
        public DateTime MyDateTime { get; set; } = new DateTime(2018, 12, 1, 9, 9, 8);
        public DateTime MyDate { get; set; } = new DateTime(2008, 9, 10);
        public DateTime[] MyDateTimes { get; set; } = [new DateTime(2008, 9, 10), new DateTime(2018, 12, 1, 9, 9, 8), new DateTime(2008, 9, 10)];
    }
}
