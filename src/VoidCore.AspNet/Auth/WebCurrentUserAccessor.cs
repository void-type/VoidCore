using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using VoidCore.Model.Auth;

namespace VoidCore.AspNet.Auth;

/// <summary>
/// Access the current user via HttpContext. Accessors can be added to DI as a singleton.
/// </summary>
public class WebCurrentUserAccessor : ICurrentUserAccessor
{
    private readonly AuthorizationSettings _authorizationSettings;
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserNameFormatStrategy _userNameFormatter;

    /// <summary>
    /// Create a new current user accessor
    /// </summary>
    /// <param name="httpContextAccessor">Accessor for the current httpcontext</param>
    /// <param name="userNameFormatter">A formatter for the user names</param>
    /// <param name="authorizationService">Policy checker for users</param>
    /// <param name="authorizationSettings">The application's authorization settings</param>
    public WebCurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IUserNameFormatStrategy userNameFormatter, IAuthorizationService authorizationService, AuthorizationSettings authorizationSettings)
    {
        _httpContextAccessor = httpContextAccessor;
        _userNameFormatter = userNameFormatter;
        _authorizationService = authorizationService;
        _authorizationSettings = authorizationSettings;
    }

    /// <inheritdoc/>
    public async Task<DomainUser> GetUser()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return new DomainUser(_userNameFormatter.Format(null), []);
        }

        var currentUser = _httpContextAccessor.HttpContext.User;
        var policies = _authorizationSettings.Policies;

        var authorizedAs = new List<string>();

        foreach (var policy in policies.Select(p => p.Key))
        {
            var result = await _authorizationService.AuthorizeAsync(currentUser, null, policy);

            if (result.Succeeded)
            {
                authorizedAs.Add(policy);
            }
        }

        var name = _userNameFormatter.Format(currentUser.Identity?.Name);

        return new DomainUser(name, authorizedAs);
    }
}
