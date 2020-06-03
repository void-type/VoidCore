using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using VoidCore.AspNet.Auth;
using VoidCore.Model.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Auth
{
    public class WebCurrentUserAccessorTests
    {
        [Fact]
        public void WebCurrentUserAccessor_returns_user_with_correct_auth()
        {
            var authSettings = new AuthorizationSettings(new Dictionary<string, List<string>>
            {
                {
                    "User",
                    new List<string> { "AppUsers", "AppAdmins" }
                },
                {
                    "Admin",
                    new List<string> { "AppAdmins" }
                }
            });

            var identityMock = new Mock<IIdentity>();
            identityMock.Setup(i => i.Name).Returns("Name@contoso.com");

            var principalMock = new Mock<ClaimsPrincipal>();
            principalMock.Setup(p => p.Identity).Returns(identityMock.Object);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(context => context.User)
                .Returns(principalMock.Object);

            var authServiceMock = new Mock<IAuthorizationService>();
            authServiceMock.Setup(a => a.AuthorizeAsync(principalMock.Object, null, "User"))
                .Returns(Task.FromResult(AuthorizationResult.Success()));
            authServiceMock.Setup(a => a.AuthorizeAsync(principalMock.Object, null, "Admin"))
                .Returns(Task.FromResult(AuthorizationResult.Failed()));

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext)
                .Returns(httpContextMock.Object);

            var accessor = new WebCurrentUserAccessor(httpContextAccessorMock.Object, new EmailUserNameFormatStrategy(), authServiceMock.Object, authSettings);

            var user = accessor.User;
            Assert.Equal("Name", user.Login);
            Assert.Contains("User", user.AuthorizedAs);
            Assert.DoesNotContain("Admin", user.AuthorizedAs);
        }
    }
}
