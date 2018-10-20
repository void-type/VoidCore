using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Moq;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.ClientApp;
using VoidCore.Model.Logging;
using Xunit;

namespace VoidCore.Test.AspNet.ClientApp
{
    public class GetWebApplicationInfoTests
    {
        [Fact]
        public void ApplicationInfoIsCreatedWithAndLogsProperInfo()
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

            var loggingServiceMock = new Mock<ILoggingService>();
            loggingServiceMock.Setup(l => l.Info(It.IsAny<string[]>()));

            var result = new GetWebApplicationInfo.Handler(appSettingsMock.Object, mockContextAccessor.Object, mockAntiforgery.Object, currentUser.Object)
                .AddPostProcessor(new GetWebApplicationInfo.Logger(loggingServiceMock.Object))
                .Handle(new GetWebApplicationInfo.Request());

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var appInfo = result.Value;

            Assert.Equal("AppName", appInfo.ApplicationName);
            Assert.Equal("UserName", appInfo.User.Name);
            Assert.Equal(new [] { "policy1", "policy2" }, appInfo.User.AuthorizedAs);
            Assert.Equal("header-name", appInfo.AntiforgeryTokenHeaderName);
            Assert.Equal("request-token", appInfo.AntiforgeryToken);

            loggingServiceMock.Verify(l => l.Info(new []
            {
                "AppName: AppName",
                "UserName: UserName",
                "UserAuthorizedAs: policy1, policy2"
            }), Times.Once());
        }
    }
}
