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

            var itemSet = queryable.ToItemSetPage(2, 3);

            Assert.Equal(2, itemSet.Count);
            Assert.Equal(2, itemSet.Items.Count());
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

            var itemSet = list.ToItemSetPage(2, 3);

            Assert.Equal(2, itemSet.Count);
            Assert.Equal(2, itemSet.Items.Count());
        }
    }
}
