using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Configuration;
using VoidCore.Model.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.ClientApp;

public class GetWebClientInfoTests
{
    [Fact]
    public async Task GetWebClientInfo_return_client_app_info()
    {
        var applicationSettings = new WebApplicationSettings()
        {
            Name = "AppName"
        };

        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.GetUser()
            .Returns(new DomainUser("UserName", ["policy1", "policy2"]));

        var contextMock = Substitute.For<HttpContext>();

        var contextAccessorMock = Substitute.For<IHttpContextAccessor>();
        contextAccessorMock.HttpContext
            .Returns(contextMock);

        var antiforgeryMock = Substitute.For<IAntiforgery>();
        antiforgeryMock.GetAndStoreTokens(Arg.Any<HttpContext>())
            .Returns(new AntiforgeryTokenSet("request-token", "cookie-token", "formFieldName", "header-name"));

        var loggerMock = Substitute.For<ILogger<GetWebClientInfo.ResponseLogger>>();

        var result = await new GetWebClientInfo.Handler(applicationSettings, contextAccessorMock, antiforgeryMock, currentUserAccessorMock)
            .AddPostProcessor(new GetWebClientInfo.ResponseLogger(loggerMock))
            .Handle(new GetWebClientInfo.Request());

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var appInfo = result.Value;

        Assert.Equal("AppName", appInfo.ApplicationName);
        Assert.Equal("UserName", appInfo.User.Login);
        Assert.Equal(new[] { "policy1", "policy2" }, appInfo.User.AuthorizedAs);
        Assert.Equal("header-name", appInfo.AntiforgeryTokenHeaderName);
        Assert.Equal("request-token", appInfo.AntiforgeryToken);
    }
}
