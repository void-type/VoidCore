using System.Text;
using VoidCore.Model.Railway.File;
using Xunit;

namespace VoidCore.Test.Model.Railway.Responses
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
