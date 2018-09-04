using Microsoft.AspNetCore.Http;
using Moq;
using System;
using VoidCore.AspNet.Logging;
using VoidCore.Model.ClientApp;
using Xunit;

namespace VoidCore.Test.AspNet
{
    public class HttpStringEventLoggerStrategyTests
    {
        [Fact]
        public void LogEventWithNestedExceptions()
        {
            var exception = new Exception("1",
                new Exception("2",
                    new Exception("3")));

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.Setup(request => request.Method).Returns("GET");
            httpRequestMock.Setup(request => request.Path).Returns(new PathString("/path/to/here"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(context => context.Request).Returns(httpRequestMock.Object);
            httpContextMock.Setup(context => context.TraceIdentifier).Returns("identifier");

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(accessor => accessor.HttpContext).Returns(httpContextMock.Object);

            var currentUser = new Mock<ICurrentUser>();
            currentUser.Setup(fmt => fmt.Name).Returns("userName");

            var strategy = new HttpStringEventLoggerStrategy(httpContextAccessorMock.Object, currentUser.Object);

            var logText = strategy.LogEvent(exception, "added12", "added23");

            var expectedPrefix = "identifier:userName:GET:/path/to/here";
            var expectedPayload = "added12 added23 Threw Exception: System.Exception: 1 System.Exception: 2 System.Exception: 3";

            Assert.Contains(expectedPrefix, logText);
            Assert.Contains(expectedPayload, logText);
        }
    }
}
