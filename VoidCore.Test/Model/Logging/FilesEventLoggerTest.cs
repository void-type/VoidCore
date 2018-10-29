using Moq;
using VoidCore.Model.Domain;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Files;
using Xunit;

namespace VoidCore.Test.Model.Logging
{
    public class FilesEventLoggerTest
    {
        [Fact]
        public void LogSimpleFile()
        {
            var file = new SimpleFile("your content here", "filename.txt");
            var result = Result.Ok(file);
            var request = "";

            var loggerMock = new Mock<ILoggingService>();
            loggerMock.Setup(l => l.Info(It.IsAny<string[]>()));

            new SimpleFileEventLogger<string>(loggerMock.Object)
                .Process(request, result);

            loggerMock.Verify(l => l.Info(
                "FileName: filename.txt"
            ), Times.Once());
        }
    }
}
