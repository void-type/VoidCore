using Microsoft.AspNetCore.Http;
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
            var appSettings = new ApplicationSettings("AppName", "https://www.contoso.com/path/base");

            var httpRequestMock = new Mock<HttpRequest>();

            var httpContextMock = new Mock<HttpContext>();

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(accessor => accessor.HttpContext)
                .Returns(httpContextMock.Object);

            var hostMock = new Mock<IHostEnvironment>();
            hostMock.Setup(h => h.EnvironmentName).Returns("TestingEnvironment");

            var variables = new WebAppVariables(httpContextAccessorMock.Object, hostMock.Object, appSettings);

            Assert.Equal("AppName", variables.AppName);
            Assert.Equal("https://www.contoso.com/path/base", variables.BaseUrl);
            Assert.Equal("TestingEnvironment", variables.Environment);
        }

        [Fact]
        public void BaseUrl_returns_httpContext_url_when_settings_empty()
        {
            var appSettings = new ApplicationSettings("AppName", "");

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(request => request.Scheme)
                .Returns("https");
            httpRequestMock.Setup(request => request.Host)
                .Returns(new HostString("www.contoso.com"));
            httpRequestMock.Setup(request => request.PathBase)
                .Returns(new PathString("/path/base"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(context => context.Request)
                .Returns(httpRequestMock.Object);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(accessor => accessor.HttpContext)
                .Returns(httpContextMock.Object);

            var hostMock = new Mock<IHostEnvironment>();
            hostMock.Setup(h => h.EnvironmentName).Returns("TestingEnvironment");

            var variables = new WebAppVariables(httpContextAccessorMock.Object, hostMock.Object, appSettings);

            Assert.Equal("AppName", variables.AppName);
            Assert.Equal("https://www.contoso.com/path/base", variables.BaseUrl);
            Assert.Equal("TestingEnvironment", variables.Environment);
        }
    }
}
