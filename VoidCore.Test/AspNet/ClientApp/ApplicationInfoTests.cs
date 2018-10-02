using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Moq;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.ClientApp;
using Xunit;

namespace VoidCore.Test.AspNet.ClientApp
{
    public class RespondWithApplicationInfoTests
    {
        [Fact]
        public void ApplicationInfoContainsProperInfo()
        {
            var appSettingsMock = new Mock<IApplicationSettings>();
            appSettingsMock.Setup(a => a.Name).Returns("AppName");

            var currentUser = new Mock<ICurrentUser>();
            currentUser
                .Setup(mock => mock.Name)
                .Returns("UserName");
            currentUser
                .Setup(mock => mock.AuthorizedAs)
                .Returns(new [] { "policy1", "policy2" });

            var mockContext = new Mock<HttpContext>();

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor
                .Setup(mock => mock.HttpContext).Returns(mockContext.Object);

            var mockAntiforgery = new Mock<IAntiforgery>();
            mockAntiforgery
                .Setup(mock => mock.GetAndStoreTokens(It.IsAny<HttpContext>()))
                .Returns(new AntiforgeryTokenSet("request-token", "cookie-token", "formFieldName", "header-name"));

            var appInfo = new WebApplicationInfo(appSettingsMock.Object, mockContextAccessor.Object, mockAntiforgery.Object, currentUser.Object);

            Assert.Equal("AppName", appInfo.ApplicationName);
            Assert.Equal("UserName", appInfo.User.Name);
            Assert.Equal(new [] { "policy1", "policy2" }, appInfo.User.AuthorizedAs);
            Assert.Equal("header-name", appInfo.AntiforgeryTokenHeaderName);
            Assert.Equal("request-token", appInfo.AntiforgeryToken);
        }
    }
}
