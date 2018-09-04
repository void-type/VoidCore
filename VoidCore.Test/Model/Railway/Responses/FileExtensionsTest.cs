using VoidCore.Model.Railway.File;
using Xunit;

namespace VoidCore.Test.Model.Railway.Responses
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
