using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ItemSet_can_be_created_from_IQueryable()
        {
            var queryable = new List<string> { "1", "2", "3", "4" }.AsQueryable();

            var itemSet = queryable.ToItemSet();

            Assert.Equal(4, itemSet.Count);
            Assert.Equal(4, itemSet.Items.Count());
        }

        [Fact]
        public void Paged_ItemSet_can_be_created_from_IQueryable()
        {
            var queryable = new List<string> { "1", "2", "3", "4", "5" }.AsQueryable();

            var options = new PaginationOptions(2, 3);

            var itemSet = queryable.ToItemSet(options);

            Assert.Contains("4", itemSet.Items);
            Assert.Contains("5", itemSet.Items);
            Assert.Equal(2, itemSet.Count);
            Assert.Equal(2, itemSet.Items.Count());
            Assert.Equal(5, itemSet.TotalCount);
        }

        [Fact]
        public void ItemSet_can_be_created_from_list()
        {
            var list = new List<string> { "1", "2", "3", "4" };

            var itemSet = list.ToItemSet();

            Assert.Equal(4, itemSet.Count);
            Assert.Equal(4, itemSet.Items.Count());
        }

        [Fact]
        public void Paged_ItemSet_can_be_created_from_list()
        {
            var list = new List<string> { "1", "2", "3", "4", "5" };

            var options = new PaginationOptions(2, 3);

            var itemSet = list.ToItemSet(options);

            Assert.Contains("4", itemSet.Items);
            Assert.Contains("5", itemSet.Items);
            Assert.Equal(2, itemSet.Count);
            Assert.Equal(2, itemSet.Items.Count());
            Assert.Equal(5, itemSet.TotalCount);
        }

        [Fact]
        public void Paged_ItemSet_can_be_created_from_page_of_a_list()
        {
            var list = new List<string> { "2", "3", "4" };

            var options = new PaginationOptions(2, 3);

            var itemSet = list.ToItemSet(options, 6);

            Assert.Contains("2", itemSet.Items);
            Assert.Contains("3", itemSet.Items);
            Assert.Contains("4", itemSet.Items);
            Assert.Equal(3, itemSet.Count);
            Assert.Equal(6, itemSet.TotalCount);
            Assert.Equal(3, itemSet.Items.Count());
        }
    }
}
