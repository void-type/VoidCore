using Microsoft.Extensions.Hosting;
using Moq;
using VoidCore.AspNet.Configuration;
using Xunit;

namespace VoidCore.Test.AspNet.Configuration
{
    public class WebAppVariablesTests
    {
        [Fact]
        public void Variables_return_all_properties()
        {
            var appSettings = new ApplicationSettings
            {
                Name = "AppName",
                BaseUrl = "https://www.contoso.com/path/base"
            };

            var hostMock = new Mock<IHostEnvironment>();
            hostMock.Setup(h => h.EnvironmentName).Returns("TestingEnvironment");

            var variables = new WebAppVariables(hostMock.Object, appSettings);

            Assert.Equal("AppName", variables.AppName);
            Assert.Equal("https://www.contoso.com/path/base", variables.BaseUrl);
            Assert.Equal("TestingEnvironment", variables.Environment);
        }
    }
}
