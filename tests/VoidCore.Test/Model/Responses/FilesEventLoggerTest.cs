using Moq;
using VoidCore.Domain;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Files;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class FilesEventLoggerTest
    {
        [Fact]
        public void SimpleFile_logs_properties()
        {
            var file = new SimpleFile("your content here", "filename.txt");
            var result = Result.Ok(file);

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            new SimpleFileEventLogger<string>(loggerMock.Object)
                .Process(string.Empty, result);

            loggerMock.Verify(l => l.Info(
                "FileName: filename.txt"
            ), Times.Once());
        }
    }
}
