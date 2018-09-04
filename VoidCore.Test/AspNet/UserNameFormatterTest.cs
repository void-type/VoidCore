using VoidCore.AspNet.ClientApp;
using Xunit;

namespace VoidCore.Test.AspNet
{
    public class UserNameFormatterTests
    {
        [Theory]
        [InlineData("Domain1\\Name", "Name")]
        [InlineData("Name", "Name")]
        [InlineData(null, "Unknown")]
        public void FormatNameFromAdLogin(string input, string expected)
        {
            var formatter = new AdLoginUserNameFormatter();

            var userName = formatter.Format(input);

            Assert.Equal(expected, userName);
        }
    }
}
