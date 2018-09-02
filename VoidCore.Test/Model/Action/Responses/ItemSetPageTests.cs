using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Action.Responses.ItemSet;
using Xunit;

namespace VoidCore.Test.Model.Action.Responses
{
    public class ItemSetPageTests
    {
        [Fact]
        public void CountEmptyItemsIsZero()
        {
            var set = new ItemSetPage<string>(new List<string>().AsEnumerable(), 2, 2);
            Assert.Equal(0, set.Count);
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
        public void PagePropertiesChecks(int totalCount, int page, int take, int expectedCount)
        {
            var set = new List<string>();

            for (var i = 0; i < totalCount; i++)
            {
                set.Add(i.ToString());
            }

            var itemSetPage = new ItemSetPage<string>(set.AsEnumerable(), page, take);

            Assert.Equal(expectedCount, itemSetPage.Count);
            Assert.Equal(expectedCount, itemSetPage.Items.Count());
            Assert.Equal(page, itemSetPage.Page);
            Assert.Equal(take, itemSetPage.Take);
            Assert.Equal(totalCount, itemSetPage.TotalCount);
        }

        [Fact]
        public void PaginateStringsReturnsProperItems()
        {
            var set = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            var itemSetPage = new ItemSetPage<int>(set.AsEnumerable(), 2, 5);

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
}