using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Responses;

public class ItemSetTests
{
    [Fact]
    public void Empty_ItemSet_count_is_zero()
    {
        var set = new ItemSet<string>(new List<string>().AsEnumerable());
        Assert.Equal(0, set.Count);

        var options = new PaginationOptions(2, 2);

        var setPaged = new ItemSet<string>(new List<string>(), options);
        Assert.Equal(0, setPaged.Count);
    }

    [Fact]
    public void Explicit_creation_without_paging_ignores_page_take_and_totalCount()
    {
        var items = new List<string> { string.Empty, string.Empty, string.Empty }.AsEnumerable();

        var options = new PaginationOptions(2, 3, false);

        var set = new ItemSet<string>(items, options, 4);
        Assert.Equal(3, set.Count);
        Assert.Equal(1, set.Page);
        Assert.Equal(3, set.Take);
        Assert.Equal(3, set.TotalCount);
        Assert.False(set.IsPagingEnabled);
    }

    [Fact]
    public void Explicit_creation_without_paging_and_empty_items_sets_take_as_one()
    {
        var items = new List<string>().AsEnumerable();

        var options = new PaginationOptions(2, 3, false);

        var set = new ItemSet<string>(items, options, 4);
        Assert.Equal(0, set.Count);
        Assert.Equal(1, set.Page);
        Assert.Equal(1, set.Take);
        Assert.Equal(0, set.TotalCount);
        Assert.False(set.IsPagingEnabled);
    }

    [Fact]
    public void Explicit_creation_sets_properties_correctly()
    {
        var items = new List<string> { string.Empty, string.Empty, string.Empty }.AsEnumerable();

        var options = new PaginationOptions(2, 3);

        var set = new ItemSet<string>(items, options, 4);
        Assert.Equal(3, set.Count);
        Assert.Equal(2, set.Page);
        Assert.Equal(3, set.Take);
        Assert.Equal(4, set.TotalCount);
        Assert.True(set.IsPagingEnabled);
    }

    [Fact]
    public void Null_items_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ItemSet<string>(null!));
        Assert.Throws<ArgumentNullException>(() => new ItemSet<string>(null!, new PaginationOptions(1, 1)));
        Assert.Throws<ArgumentNullException>(() => new ItemSet<string>(null!, new PaginationOptions(1, 1), 1));
    }

    [Theory]
    [InlineData(0, 1, 1, 0)]
    [InlineData(100, 1, 10, 10)]
    [InlineData(100, 10, 10, 10)]
    [InlineData(100, 11, 10, 0)]
    [InlineData(105, 11, 10, 5)]
    [InlineData(15, 0, 5, 5)]
    [InlineData(15, -1, 5, 5)]
    [InlineData(15, 2, 0, 0)]
    [InlineData(15, 2, -1, 0)]
    public void Paged_ItemSet_property_checks(int totalCount, int page, int take, int expectedCount)
    {
        var set = new List<string>();

        for (var i = 0; i < totalCount; i++)
        {
            set.Add(i.ToString());
        }

        var itemSetPage = new ItemSet<string>(set, new PaginationOptions(page, take));

        Assert.Equal(expectedCount, itemSetPage.Count);
        Assert.Equal(expectedCount, itemSetPage.Items.Count());
        Assert.Equal(page, itemSetPage.Page);
        Assert.Equal(take, itemSetPage.Take);
        Assert.Equal(totalCount, itemSetPage.TotalCount);
    }

    [Fact]
    public void Pagination_returns_correct_range_of_items()
    {
        var set = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        var itemSetPage = new ItemSet<int>(set, new PaginationOptions(2, 5));

        Assert.Contains(6, itemSetPage.Items);
        Assert.Contains(7, itemSetPage.Items);
        Assert.Contains(8, itemSetPage.Items);
        Assert.Contains(9, itemSetPage.Items);
        Assert.Contains(10, itemSetPage.Items);

        Assert.Equal(2, itemSetPage.Page);
        Assert.Equal(5, itemSetPage.Take);
        Assert.Equal(5, itemSetPage.Count);
        Assert.Equal(15, itemSetPage.TotalCount);
    }
}
