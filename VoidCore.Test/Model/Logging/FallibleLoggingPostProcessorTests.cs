using Moq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using Xunit;

namespace VoidCore.Test.Model.Logging
{
    public class FallibleLoggingPostProcessorTests
    {
        [Fact]
        public void LoggerIsCalledOnFailure()
        {
            var result = Result.Fail<string>("oops");

            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Warn(It.IsAny<string[]>()));

            var processor = new FallibleLoggingPostProcessor<string, string>(loggerMock.Object);

            processor.Process(request, result);

            loggerMock.Verify(l => l.Warn(It.IsAny<string[]>()), Times.Once());
        }
    }
}
