using VoidCore.AspNet.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Users
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
    }
}
