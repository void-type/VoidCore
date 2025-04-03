using VoidCore.Model.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Auth;

public class UserNameFormatStrategyTests
{
    [Theory]
    [InlineData("Domain1\\Name", "Name")]
    [InlineData("Name", "Name")]
    [InlineData(null, "Unknown")]
    public void Format_name_from_login(string? input, string expected)
    {
        var formatter = new AdLoginUserNameFormatStrategy();
        var userName = formatter.Format(input);
        Assert.Equal(expected, userName);
    }

    [Theory]
    [InlineData("Name@contoso.com", "Name")]
    [InlineData("Name", "Name")]
    [InlineData(null, "Unknown")]
    public void Format_name_from_email(string? input, string expected)
    {
        var formatter = new EmailUserNameFormatStrategy();
        var userName = formatter.Format(input);
        Assert.Equal(expected, userName);
    }
}
