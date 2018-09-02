using VoidCore.Model.Action.Responses.File;
using Xunit;

namespace VoidCore.Test.Model.Action.Responses
{
    public class FileExtensionsTests
    {
        [Fact]
        public void SimpleFileGetLogText()
        {
            var logText = new SimpleFile("file content here", "filename.txt").GetLogText();
            var expected = new[] { "FileName: filename.txt" };
            Assert.Equal(expected, logText);
        }
    }
}