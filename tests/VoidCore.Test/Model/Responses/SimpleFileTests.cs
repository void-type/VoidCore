using System;
using System.Text;
using VoidCore.Model.Responses.Files;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class SimpleFileTests
    {
        private const string TestString = "file content \n \r\n here 㯪 שלום עולם 你好，世界！";

        [Fact]
        public void SimpleFileConstructFromString()
        {
            var file = new SimpleFile(TestString, "filename.txt");
            var contentString = Encoding.UTF8.GetString(file.Content.AsBytes);
            Assert.Equal(TestString, contentString);
            Assert.Equal(TestString, file.Content.ToString());
            Assert.Equal("filename.txt", file.Name);
        }

        [Fact]
        public void SimpleFileConstructFromBytes()
        {
            var file = new SimpleFile(Encoding.UTF8.GetBytes(TestString), "filename.txt");
            var contentString = Encoding.UTF8.GetString(file.Content.AsBytes);
            Assert.Equal(TestString, contentString);
            Assert.Equal(TestString, file.Content.ToString());
            Assert.Equal("filename.txt", file.Name);
        }

        [Fact]
        public void FilesAreEqualWhenNameAndContentsEqual()
        {
            var file1 = new SimpleFile(Encoding.UTF8.GetBytes(TestString), "filename.txt");
            var file2 = new SimpleFile(TestString, "filename.txt");

            Assert.Equal(file1, file2);
        }

        [Fact]
        public void FilesAreNotEqualWhenNamesDifferent()
        {
            var file1 = new SimpleFile(Encoding.UTF8.GetBytes(TestString), "filename2.txt");
            var file2 = new SimpleFile(TestString, "filename.txt");

            Assert.NotEqual(file1, file2);
        }

        [Fact]
        public void FilesAreNotEqualWhenContentDifferent()
        {
            var file1 = new SimpleFile(Encoding.UTF8.GetBytes(TestString), "filename.txt");
            var file2 = new SimpleFile(TestString + "extra", "filename.txt");

            Assert.NotEqual(file1, file2);
        }

        [Fact]
        public void CreatingFileWithNullContentThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleFile((byte[])null, "some.txt"));
        }

        [Fact]
        public void CreatingFileWithNullNameThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleFile(string.Empty, null));
        }

        [Fact]
        public void CreatingFileWithEmptyNameThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new SimpleFile(string.Empty, string.Empty));
        }
    }
}
