using System.Text;
using VoidCore.Model.Action.Responses.File;
using Xunit;

namespace VoidCore.Test.Model.Action.Responses.File
{
    public class SimpleFileTests
    {
        [Fact]
        public void SimpleFileConstructFromString()
        {
            var file = new SimpleFile("file content here", "filename.txt");
            var contentString = Encoding.UTF8.GetString(file.Content);
            Assert.Equal("file content here", contentString);
            Assert.Equal("filename.txt", file.Name);
        }
    }
}
