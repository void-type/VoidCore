using Microsoft.AspNetCore.Http;
using Moq;
using System;
using VoidCore.AspNet.Logging;
using VoidCore.Model.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Logging
{
    public class HttpRequestLoggingStrategyTests
    {
        [Fact]
        public void Log_event_with_nested_exception()
        {
            var exception = new Exception("1",
                new Exception("2",
                    new Exception("3")));

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(request => request.Method)
                .Returns("GET");
            httpRequestMock.Setup(request => request.Path)
                .Returns(new PathString("/path/to/here"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(context => context.Request)
                .Returns(httpRequestMock.Object);
            httpContextMock.Setup(context => context.TraceIdentifier)
                .Returns("identifier");

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(accessor => accessor.HttpContext)
                .Returns(httpContextMock.Object);

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock.Setup(mock => mock.User)
                .Returns(new DomainUser("userName", new string[] { }));

            var strategy = new HttpRequestLoggingStrategy(httpContextAccessorMock.Object, currentUserAccessorMock.Object);

            var logText = strategy.Log(exception, "added12", "added23");

            const string expectedPrefix = "identifier:userName:GET:/path/to/here";
            const string expectedPayload = "added12 added23 Threw Exception: System.Exception: 1 System.Exception: 2 System.Exception: 3";

            Assert.Contains(expectedPrefix, logText);
            Assert.Contains(expectedPayload, logText);
        }

        [Fact]
        public void Log_event_with_null_exceptions()
        {
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(request => request.Method)
                .Returns("GET");
            httpRequestMock.Setup(request => request.Path)
                .Returns(new PathString("/path/to/here"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(context => context.Request)
                .Returns(httpRequestMock.Object);
            httpContextMock.Setup(context => context.TraceIdentifier)
                .Returns("identifier");

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(accessor => accessor.HttpContext)
                .Returns(httpContextMock.Object);

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock.Setup(mock => mock.User)
                .Returns(new DomainUser("userName", new string[] { }));

            var strategy = new HttpRequestLoggingStrategy(httpContextAccessorMock.Object, currentUserAccessorMock.Object);
            Exception exception = null;

            var logText = strategy.Log(exception, "added12", "added23");

            const string expectedPrefix = "identifier:userName:GET:/path/to/here";
            const string expectedPayload = "added12 added23";

            Assert.Contains(expectedPrefix, logText);
            Assert.Contains(expectedPayload, logText);
        }
    }
}
