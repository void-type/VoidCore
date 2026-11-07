using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Responses;

public class PaginationOptionsTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(-5, 1)]
    [InlineData(3, 3)]
    public void Page_is_at_least_one(int page, int expected)
    {
        Assert.Equal(expected, new PaginationOptions(page, 10).Page);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(25, 25)]
    [InlineData(PaginationOptions.MaxTake + 1, PaginationOptions.MaxTake)]
    [InlineData(int.MaxValue, PaginationOptions.MaxTake)]
    public void Take_is_bounded(int take, int expected)
    {
        Assert.Equal(expected, new PaginationOptions(1, take).Take);
    }

    [Fact]
    public void Skip_does_not_overflow_for_huge_page_numbers()
    {
        var options = new PaginationOptions(int.MaxValue, PaginationOptions.MaxTake);

        Assert.Equal(int.MaxValue, options.Skip);
    }

    [Fact]
    public void Skip_is_the_number_of_items_before_the_page()
    {
        Assert.Equal(0, new PaginationOptions(1, 10).Skip);
        Assert.Equal(20, new PaginationOptions(3, 10).Skip);
        Assert.Equal(0, PaginationOptions.None.Skip);
    }

    [Fact]
    public void Invalid_page_input_pages_without_throwing()
    {
        var set = Enumerable.Range(1, 5).AsQueryable();

        var page = set.GetPage(new PaginationOptions(-10, 2)).ToList();

        Assert.Equal([1, 2], page);
    }
}
