using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Responses.ItemSet;
using Xunit;

namespace VoidCore.Test.Model.Railway.Responses
{
    public class ItemSetTests
    {
        [Fact]
        public void CountEmptyItemsIsZero()
        {
            var set = new ItemSet<string>(new List<string>().AsEnumerable());
            Assert.Equal(0, set.Count);
        }

        [Fact]
        public void CountThreeItemsIsThree()
        {
            var items = new List<string>() { "", "", "" }.AsEnumerable();
            var set = new ItemSet<string>(items);
            Assert.Equal(3, set.Count);
        }
    }
}
