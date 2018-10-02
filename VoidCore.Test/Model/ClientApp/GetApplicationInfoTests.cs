using Moq;
using VoidCore.Model.ClientApp;
using VoidCore.Model.Logging;
using Xunit;

namespace VoidCore.Test.Model.ClientApp
{
    public class GetApplicationInfoTests
    {
        [Fact]
        public void HandlerReturnsOkResultOfIApplicationInfo()
        {
            var appInfoMock = new Mock<GetApplicationInfo.IApplicationInfo>();
            var result = new GetApplicationInfo.Handler(appInfoMock.Object)
                .Handle(new GetApplicationInfo.Request());

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void LoggingLogsAppInfo()
        {
            var userMock = new Mock<ICurrentUser>();
            userMock.Setup(u => u.Name).Returns("userName");
            userMock.Setup(u => u.AuthorizedAs).Returns(new [] { "policy1", "policy2" });

            var appInfoMock = new Mock<GetApplicationInfo.IApplicationInfo>();
            appInfoMock.Setup(a => a.ApplicationName).Returns("appName");
            appInfoMock.Setup(a => a.User).Returns(userMock.Object);

            var result = new GetApplicationInfo.Handler(appInfoMock.Object)
                .Handle(new GetApplicationInfo.Request());

            var loggingServiceMock = new Mock<ILoggingService>();
            loggingServiceMock.Setup(l => l.Info(It.IsAny<string[]>()));

            new GetApplicationInfo.Logging(loggingServiceMock.Object).Process(new GetApplicationInfo.Request(), result);

            loggingServiceMock.Verify(l => l.Info(new []
            {
                "AppName: appName",
                "UserName: userName",
                "UserAuthorizedAs: policy1, policy2"
            }), Times.Once());
        }

    }
}
