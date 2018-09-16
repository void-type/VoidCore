using System.Text;
using VoidCore.Model.Responses.File;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class SimpleFileTests
    {
        private const string testString = "file content \n \r\n here 㯪 שלום עולם 你好，世界！";

        [Fact]
        public void SimpleFileConstructFromString()
        {
            var file = new SimpleFile(testString, "filename.txt");
            var contentString = Encoding.UTF8.GetString(file.Content);
            Assert.Equal(testString, contentString);
            Assert.Equal(testString, file.ContentAsString);
            Assert.Equal("filename.txt", file.Name);
        }

        [Fact]
        public void SimpleFileConstructFromBytes()
        {
            var file = new SimpleFile(Encoding.UTF8.GetBytes(testString), "filename.txt");
            var contentString = Encoding.UTF8.GetString(file.Content);
            Assert.Equal(testString, contentString);
            Assert.Equal(testString, file.ContentAsString);
            Assert.Equal("filename.txt", file.Name);
        }
    }
}
