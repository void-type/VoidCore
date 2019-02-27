using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void FromIQueryableToItemSet()
        {
            var queryable = new List<string> { "1", "2", "3", "4" }.AsQueryable();

            var itemSet = queryable.ToItemSet();

            Assert.Equal(4, itemSet.Count);
            Assert.Equal(4, itemSet.Items.Count());
        }

        [Fact]
        public void FromIQueryableToItemSetPage()
        {
            var queryable = new List<string> { "1", "2", "3", "4", "5" }.AsQueryable();

            var itemSet = queryable.ToItemSet(2, 3);

            Assert.Contains("4", itemSet.Items);
            Assert.Contains("5", itemSet.Items);
            Assert.Equal(2, itemSet.Count);
            Assert.Equal(2, itemSet.Items.Count());
            Assert.Equal(5, itemSet.TotalCount);
        }

        [Fact]
        public void FromListToItemSet()
        {
            var list = new List<string> { "1", "2", "3", "4" };

            var itemSet = list.ToItemSet();

            Assert.Equal(4, itemSet.Count);
            Assert.Equal(4, itemSet.Items.Count());
        }

        [Fact]
        public void FromListToItemSetPage()
        {
            var list = new List<string> { "1", "2", "3", "4", "5" };

            var itemSet = list.ToItemSet(2, 3);

            Assert.Contains("4", itemSet.Items);
            Assert.Contains("5", itemSet.Items);
            Assert.Equal(2, itemSet.Count);
            Assert.Equal(2, itemSet.Items.Count());
            Assert.Equal(5, itemSet.TotalCount);
        }

        [Fact]
        public void FromPagedListToItemSet()
        {
            var list = new List<string> { "2", "3", "4" };

            var itemSet = list.ToItemSet(2, 3, 6);

            Assert.Contains("2", itemSet.Items);
            Assert.Contains("3", itemSet.Items);
            Assert.Contains("4", itemSet.Items);
            Assert.Equal(3, itemSet.Count);
            Assert.Equal(6, itemSet.TotalCount);
            Assert.Equal(3, itemSet.Items.Count());
        }
    }
}
