using VoidCore.AspNet.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Auth
{
    public class UserNameFormatStrategyTests
    {
        [Theory]
        [InlineData("Domain1\\Name", "Name")]
        [InlineData("Name", "Name")]
        [InlineData(null, "Unknown")]
        public void FormatNameFromAdLogin(string input, string expected)
        {
            var formatter = new AdLoginUserNameFormatStrategy();
            var userName = formatter.Format(input);
            Assert.Equal(expected, userName);
        }

        [Theory]
        [InlineData("Name@contoso.com", "Name")]
        [InlineData("Name", "Name")]
        [InlineData(null, "Unknown")]
        public void FormatNameFromEmail(string input, string expected)
        {
            var formatter = new EmailUserNameFormatStrategy();
            var userName = formatter.Format(input);
            Assert.Equal(expected, userName);
        }
    }
}
