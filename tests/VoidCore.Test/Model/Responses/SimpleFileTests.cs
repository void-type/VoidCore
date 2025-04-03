using System.Text;
using VoidCore.Model.Responses.Files;
using Xunit;

namespace VoidCore.Test.Model.Responses;

public class SimpleFileTests
{
    private const string TestString = "file content \n \r\n here 㯪 שלום עולם 你好，世界！";

    [Fact]
    public void SimpleFile_can_be_constructed_from_string()
    {
        var file = new SimpleFile(TestString, "filename.txt");
        var contentString = Encoding.UTF8.GetString(file.Content.AsBytes);
        Assert.Equal(TestString, contentString);
        Assert.Equal(TestString, file.Content.ToString());
        Assert.Equal("filename.txt", file.Name);
    }

    [Fact]
    public void SimpleFile_can_be_constructed_from_bytes()
    {
        var file = new SimpleFile(Encoding.UTF8.GetBytes(TestString), "filename.txt");
        var contentString = Encoding.UTF8.GetString(file.Content.AsBytes);
        Assert.Equal(TestString, contentString);
        Assert.Equal(TestString, file.Content.ToString());
        Assert.Equal("filename.txt", file.Name);
    }

    [Fact]
    public void Files_are_equal_when_content_and_filenames_are_equal()
    {
        var file1 = new SimpleFile(Encoding.UTF8.GetBytes(TestString), "filename.txt");
        var file2 = new SimpleFile(TestString, "filename.txt");

        Assert.Equal(file1, file2);
    }

    [Fact]
    public void Files_are_not_equal_if_names_are_different()
    {
        var file1 = new SimpleFile(Encoding.UTF8.GetBytes(TestString), "filename2.txt");
        var file2 = new SimpleFile(TestString, "filename.txt");

        Assert.NotEqual(file1, file2);
    }

    [Fact]
    public void Files_are_not_equal_if_content_is_different()
    {
        var file1 = new SimpleFile(Encoding.UTF8.GetBytes(TestString), "filename.txt");
        var file2 = new SimpleFile(TestString + "extra", "filename.txt");

        Assert.NotEqual(file1, file2);
    }

    [Fact]
    public void Creating_file_with_null_content_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new SimpleFile((byte[])null!, "some.txt"));
    }

    [Fact]
    public void Creating_file_with_null_name_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new SimpleFile(string.Empty, null!));
    }

    [Fact]
    public void Creating_file_with_empty_name_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new SimpleFile(string.Empty, string.Empty));
    }
}
