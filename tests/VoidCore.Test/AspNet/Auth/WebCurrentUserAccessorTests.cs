using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Security.Claims;
using System.Security.Principal;
using VoidCore.AspNet.Auth;
using VoidCore.Model.Auth;
using Xunit;

namespace VoidCore.Test.AspNet.Auth;

public class WebCurrentUserAccessorTests
{
    [Fact]
    public async Task WebCurrentUserAccessor_returns_user_with_correct_auth()
    {
        var authSettings = new AuthorizationSettings
        {
            Policies = new Dictionary<string, List<string>>
                {
                    {
                        "User",
                        new List<string> { "AppUsers", "AppAdmins" }
                    },
                    {
                        "Admin",
                        new List<string> { "AppAdmins" }
                    }
                }
        };

        var identityMock = Substitute.For<IIdentity>();
        identityMock.Name.Returns("Name@contoso.com");

        var principalMock = Substitute.For<ClaimsPrincipal>();
        principalMock.Identity.Returns(identityMock);

        var httpContextMock = Substitute.For<HttpContext>();
        httpContextMock.User.Returns(principalMock);

        var authServiceMock = Substitute.For<IAuthorizationService>();
        authServiceMock
            .AuthorizeAsync(principalMock, null, "User")
            .Returns(Task.FromResult(AuthorizationResult.Success()));
        authServiceMock
            .AuthorizeAsync(principalMock, null, "Admin")
            .Returns(Task.FromResult(AuthorizationResult.Failed()));

        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        httpContextAccessorMock.HttpContext.Returns(httpContextMock);

        var accessor = new WebCurrentUserAccessor(httpContextAccessorMock, new EmailUserNameFormatStrategy(), authServiceMock, authSettings);

        var user = await accessor.GetUser();
        Assert.Equal("Name", user.Login);
        Assert.Contains("User", user.AuthorizedAs);
        Assert.DoesNotContain("Admin", user.AuthorizedAs);
    }

    [Fact]
    public async Task WebCurrentUserAccessor_returns_user_with_no_permissions_if_context_is_null()
    {
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        httpContextAccessorMock.HttpContext
            .Returns((HttpContext)null!);

        var accessor = new WebCurrentUserAccessor(httpContextAccessorMock, new EmailUserNameFormatStrategy(), null!, null!);

        var user = await accessor.GetUser();

        Assert.Empty(user.AuthorizedAs);
    }
}
