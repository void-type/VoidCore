using System.Collections.Generic;
using Moq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class CollectionsLoggingExtensionsTests
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

        [Fact]
        public void LogItemSet()
        {
            var result = Result.Ok(new List<string>() { "one", "two" }.ToItemSet());

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new ItemSetEventLogger<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Count: 2"), Times.Once());
        }

        [Fact]
        public void LogItemSetPage()
        {
            var result = Result.Ok(new List<string>() { "one", "two" }.ToItemSetPage(1, 1));

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var processor = new ItemSetPageEventLogger<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Info("Count: 1", "Page: 1", "Take: 1", "TotalCount: 2"), Times.Once());
        }
    }
}
