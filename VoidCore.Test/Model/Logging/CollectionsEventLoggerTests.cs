using Moq;
using VoidCore.Model.Domain;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Logging
{
    public class CollectionsEventLoggerTests
    {
        [Fact]
        public void LogItemSet()
        {
            var itemSetMock = new Mock<IItemSet<string>>();
            itemSetMock.SetupGet(set => set.Count).Returns(7);
            var result = Result.Ok(itemSetMock.Object);

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            new ItemSetEventLogger<string, string>(loggerMock.Object)
                .Process(request, result);

            loggerMock.Verify(l => l.Info("Count: 7"), Times.Once());
        }

        [Fact]
        public void LogItemSetPage()
        {
            var itemSetMock = new Mock<IItemSetPage<string>>();
            itemSetMock.SetupGet(set => set.Count).Returns(7);
            itemSetMock.SetupGet(set => set.Page).Returns(8);
            itemSetMock.SetupGet(set => set.Take).Returns(9);
            itemSetMock.SetupGet(set => set.TotalCount).Returns(10);

            var result = Result.Ok(itemSetMock.Object);

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            new ItemSetPageEventLogger<string, string>(loggerMock.Object)
                .Process(request, result);

            loggerMock.Verify(l => l.Info(
                "Count: 7",
                "Page: 8",
                "Take: 9",
                "TotalCount: 10"), Times.Once());
        }
    }
}