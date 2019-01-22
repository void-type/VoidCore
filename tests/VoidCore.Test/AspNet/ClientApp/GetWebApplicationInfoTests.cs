using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Moq;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Logging;
using VoidCore.Model.Users;
using Xunit;

namespace VoidCore.Test.AspNet.ClientApp
{
    public class GetWebApplicationInfoTests
    {
        [Fact]
        public async void ApplicationInfoIsCreatedWithAndLogsProperInfo()
        {
            var appSettings = new ApplicationSettings("AppName");

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock
                .Setup(mock => mock.User)
                .Returns(new DomainUser("UserName", new [] { "policy1", "policy2" }));

            var contextMock = new Mock<HttpContext>();

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            contextAccessorMock
                .Setup(mock => mock.HttpContext)
                .Returns(contextMock.Object);

            var antiforgeryMock = new Mock<IAntiforgery>();
            antiforgeryMock
                .Setup(mock => mock.GetAndStoreTokens(It.IsAny<HttpContext>()))
                .Returns(new AntiforgeryTokenSet("request-token", "cookie-token", "formFieldName", "header-name"));

            var loggingServiceMock = new Mock<ILoggingService>();
            loggingServiceMock
                .Setup(l => l.Info(It.IsAny<string[]>()));

            var result = await new GetWebApplicationInfo.Handler(appSettings, contextAccessorMock.Object, antiforgeryMock.Object, currentUserAccessorMock.Object)
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
