using Moq;
using VoidCore.Domain;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Collections;
using Xunit;

namespace VoidCore.Test.Model.Logging
{
    public class CollectionsEventLoggerTests
    {
        [Fact]
        public void ItemSet_logs_properties()
        {
            var itemSetMock = new Mock<IItemSet<string>>();
            itemSetMock.SetupGet(set => set.Count).Returns(7);
            itemSetMock.SetupGet(set => set.IsPagingEnabled).Returns(true);
            itemSetMock.SetupGet(set => set.Page).Returns(8);
            itemSetMock.SetupGet(set => set.Take).Returns(9);
            itemSetMock.SetupGet(set => set.TotalCount).Returns(10);

            var result = Result.Ok(itemSetMock.Object);

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            new ItemSetEventLogger<string, string>(loggerMock.Object)
                .Process(string.Empty, result);

            loggerMock.Verify(l => l.Info(
                "Count: 7",
                "IsPagingEnabled: True",
                "Page: 8",
                "Take: 9",
                "TotalCount: 10"), Times.Once());
        }
    }
}
