using System;
using VoidCore.Model.Text;
using Xunit;

namespace VoidCore.Test.Model.Text;

public class TextHelpersTests
{
    [Fact]
    public void Can_make_lines_from_object_properties_with_dates_and_arrays()
    {
        var strings = TextHelpers.PrintObject(new TestObject(), "MM/dd/yyyy h:mm tt");

        Assert.Equal(new[]{
                "MyString: Hello World",
                "MyInteger: 201",
                "MyStrings: 5, 6, 7, 8",
                "MyIntegers: 1, 2, 3, 4",
                "MyDateTime: 12/01/2018 9:09 AM",
                "MyDate: 09/10/2008 12:00 AM",
                "MyDateTimes: 09/10/2008 12:00 AM, 12/01/2018 9:09 AM, 09/10/2008 12:00 AM",
                }, strings);
    }

    [Fact]
    public void Can_format_dates_to_ISO8601_by_default()
    {
        var strings = TextHelpers.PrintObject(new TestObject());

        Assert.Equal(new[]{
                "MyString: Hello World",
                "MyInteger: 201",
                "MyStrings: 5, 6, 7, 8",
                "MyIntegers: 1, 2, 3, 4",
                "MyDateTime: 2018-12-01T09:09:08",
                "MyDate: 2008-09-10T00:00:00",
                "MyDateTimes: 2008-09-10T00:00:00, 2018-12-01T09:09:08, 2008-09-10T00:00:00",
                }, strings);
    }

    [Fact]
    public void Can_make_web_links()
    {
        var link = TextHelpers.Link("Click me", "https://www.contoso.com", "recipes", "1");

        Assert.Equal("<a href=\"https://www.contoso.com/recipes/1\">Click me</a>", link);
    }

    [Fact]
    public void Can_split_text_on_any_new_line()
    {
        var strings = TextHelpers.SplitOnNewLine("Click me\nhttps://www.contoso.com\r\nrecipes\\1");

        Assert.Equal(new[] { "Click me", "https://www.contoso.com", "recipes\\1" }, strings);
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
    public void Slugify_returns_empty_string_when_input_null_or_whitespace(string input)
    {
        var actual = input.Slugify();

        Assert.Equal(string.Empty, actual);
    }

    [Fact]
    public void Slugify_removes_extra_spaces_accents_and_special_chars()
    {
        var actual = "The   quick; Brown < Fo>X Júmp,ed over the lÅzÿ Dòg 大2".Slugify();
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

    public class TestObject
    {
        public string MyString { get; set; } = "Hello World";
        public int MyInteger { get; set; } = 201;
        public string[] MyStrings { get; set; } = new[] { "5", "6", "7", "8" };
        public int[] MyIntegers { get; set; } = new[] { 1, 2, 3, 4 };
        public DateTime MyDateTime { get; set; } = new DateTime(2018, 12, 1, 9, 9, 8);
        public DateTime MyDate { get; set; } = new DateTime(2008, 9, 10);
        public DateTime[] MyDateTimes { get; set; } = new[] { new DateTime(2008, 9, 10), new DateTime(2018, 12, 1, 9, 9, 8), new DateTime(2008, 9, 10) };
    }
}
