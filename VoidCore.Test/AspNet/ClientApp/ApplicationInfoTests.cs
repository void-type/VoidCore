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
        public void RespondWithAppInfo()
        {
            var appSettings = new ApplicationSettings { Name = "AppName" };

            var currentUser = new Mock<ICurrentUser>();
            currentUser
                .Setup(mock => mock.Name)
                .Returns("UserName");
            currentUser
                .Setup(mock => mock.Policies)
                .Returns(new[] {"policy1", "policy2"});

            var mockContext = new Mock<HttpContext>();

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor
                .Setup(mock => mock.HttpContext).Returns(mockContext.Object);

            var mockAntiforgery = new Mock<IAntiforgery>();
            mockAntiforgery
                .Setup(mock => mock.GetAndStoreTokens(It.IsAny<HttpContext>()))
                .Returns(new AntiforgeryTokenSet("request-token", "cookie-token", "formFieldName", "header-name"));

            var appInfo = new ApplicationInfo(appSettings, mockContextAccessor.Object, mockAntiforgery.Object, currentUser.Object);

            Assert.Equal("AppName", appInfo.ApplicationName);
            Assert.Equal("UserName", appInfo.UserName);
            Assert.Equal(new[] {"policy1", "policy2"}, appInfo.UserPolicies);
            Assert.Equal("header-name", appInfo.AntiforgeryTokenHeaderName);
            Assert.Equal("request-token", appInfo.AntiforgeryToken);
        }
    }
}