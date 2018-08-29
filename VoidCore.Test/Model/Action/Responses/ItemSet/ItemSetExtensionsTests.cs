using Moq;
using System.Collections.Generic;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Validation;
using Xunit;

namespace VoidCore.Test.Model.Action.Responses.ItemSet
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
        public void ItemSetOfValidationErrorsGetLogText()
        {
            var innerErrors = new List<IValidationError>
            {
                new ValidationError("message 1.", "field 1"),
                new ValidationError("message 2.", "field 1"),
                new ValidationError("message 3.", "field 1"),
                new ValidationError("message 4.", "field 1"),
                new ValidationError("message 5."),
                new ValidationError("")
            };

            var itemSetMock = new Mock<IItemSet<IValidationError>>();
            itemSetMock.SetupGet(set => set.Count).Returns(9);
            itemSetMock.SetupGet(set => set.Items).Returns(innerErrors);

            var logText = itemSetMock.Object.GetLogText();

            var expected = new []
            {
                "Count: 9",
                "ValidationErrors:",
                "message 1.",
                "message 2.",
                "message 3.",
                "message 4.",
                "message 5.",
                ""
            };
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
