using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Moq;
using VoidCore.AspNet.Action;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Authorization;
using Xunit;

namespace VoidCore.Test.AspNet
{
    public class RespondWithApplicationInfoTests
    {
        [Fact]
        public void RespondWithAppInfo()
        {
            IApplicationInfo appInfo = null;

            var responderMock = new Mock<IActionResponder>();
            responderMock
                .Setup(mock => mock.WithSuccess(It.IsAny<IApplicationInfo>(), It.IsAny<string[]>()))
                .Callback((object info, string[] log) =>
                {
                    appInfo = (IApplicationInfo) info;
                });

            var appSettings = new ApplicationSettings { Name = "AppName" };

            var currentUser = new Mock<ICurrentUser>();
            currentUser
                .Setup(mock => mock.Name)
                .Returns("UserName");

            var mockContext = new Mock<HttpContext>();

            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            mockContextAccessor
                .Setup(mock => mock.HttpContext).Returns(mockContext.Object);

            var mockAntiforgery = new Mock<IAntiforgery>();
            mockAntiforgery
                .Setup(mock => mock.GetAndStoreTokens(It.IsAny<HttpContext>()))
                .Returns(new AntiforgeryTokenSet("request-token", "cookie-token", "formFieldName", "header-name"));

            new RespondWithApplicationInfo(appSettings, mockContextAccessor.Object, mockAntiforgery.Object, currentUser.Object)
                .Perform(responderMock.Object);

            responderMock.Verify(mock => mock.WithSuccess(It.IsAny<ApplicationInfo>(), It.IsAny<string[]>()), Times.Once());

            Assert.Equal("AppName", appInfo.ApplicationName);
            Assert.Equal("UserName", appInfo.UserName);
            Assert.Equal("header-name", appInfo.AntiforgeryTokenHeaderName);
            Assert.Equal("request-token", appInfo.AntiforgeryToken);
        }
    }
}
