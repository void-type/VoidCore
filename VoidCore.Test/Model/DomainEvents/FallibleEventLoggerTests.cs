using Moq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using Xunit;

namespace VoidCore.Test.Model.DomainEvents
{
    public class FallibleEventLoggerTests
    {
        [Fact]
        public void LogFallible()
        {
            var result = Result.Fail<string>("oops");

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Warn(It.IsAny<string[]>()));

            var processor = new FallibleEventLogger<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Warn(new[] {"Count: 1", "Failures: oops"}), Times.Once());
        }
    }
}
