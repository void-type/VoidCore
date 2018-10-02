using VoidCore.Model.Responses.Files;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class FileExtensionsTests
    {
        [Fact]
        public void SimpleFileGetLogText()
        {
            var logText = new SimpleFile("file content here", "filename.txt").GetLogText();
            var expected = new [] { "FileName: filename.txt" };
            Assert.Equal(expected, logText);
        }
    }
}
