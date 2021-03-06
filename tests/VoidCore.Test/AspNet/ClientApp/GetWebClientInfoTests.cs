﻿using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Auth;
using VoidCore.Model.Configuration;
using VoidCore.Model.Logging;
using Xunit;

namespace VoidCore.Test.AspNet.ClientApp
{
    public class GetWebClientInfoTests
    {
        [Fact]
        public async Task GetWebClientInfo_return_client_app_info()
        {
            var wepAppVariablesMock = new Mock<IWebAppVariables>();
            wepAppVariablesMock.Setup(v => v.AppName).Returns("AppName");

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock
                .Setup(mock => mock.User)
                .Returns(new DomainUser("UserName", new[] { "policy1", "policy2" }));

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

            var result = await new GetWebClientInfo.Handler(wepAppVariablesMock.Object, contextAccessorMock.Object, antiforgeryMock.Object, currentUserAccessorMock.Object)
                .AddPostProcessor(new GetWebClientInfo.Logger(loggingServiceMock.Object))
                .Handle(new GetWebClientInfo.Request());

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            var appInfo = result.Value;

            Assert.Equal("AppName", appInfo.ApplicationName);
            Assert.Equal("UserName", appInfo.User.Login);
            Assert.Equal(new[] { "policy1", "policy2" }, appInfo.User.AuthorizedAs);
            Assert.Equal("header-name", appInfo.AntiforgeryTokenHeaderName);
            Assert.Equal("request-token", appInfo.AntiforgeryToken);

            loggingServiceMock.Verify(l => l.Info("AppName: AppName", "UserName: UserName", "UserAuthorizedAs: policy1, policy2"), Times.Once());
        }
    }
}
