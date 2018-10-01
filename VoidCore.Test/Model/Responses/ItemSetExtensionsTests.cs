using Moq;
using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class ItemSetExtensionsTests
    {
        [Fact]
        public void ItemSetGetLogText()
        {
            var itemSetMock = new Mock<IItemSet<int>>();
            itemSetMock.SetupGet(set => set.Count).Returns(7);
            var logText = itemSetMock.Object.GetLogText();
            var expected = new [] { "Count: 7" };

            Assert.Equal(expected, logText);
        }

        [Fact]
        public void ItemSetPageGetLogText()
        {
            var itemSetMock = new Mock<IItemSetPage<int>>();
            itemSetMock.SetupGet(set => set.Count).Returns(7);
            itemSetMock.SetupGet(set => set.Page).Returns(8);
            itemSetMock.SetupGet(set => set.Take).Returns(9);
            itemSetMock.SetupGet(set => set.TotalCount).Returns(10);

            var logText = itemSetMock.Object.GetLogText();

            var expected = new []
            {
                "Count: 7",
                "Page: 8",
                "Take: 9",
                "TotalCount: 10"
            };
            Assert.Equal(expected, logText);
        }
    }
}
