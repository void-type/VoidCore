using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VoidCore.Model.Auth;
using VoidCore.Model.Guards;

namespace VoidCore.AspNet.Auth;

/// <summary>
/// Configuration for authentication and authorization.
/// </summary>
public static class AuthServiceCollectionExtensions
{
    /// <summary>
    /// Setup an authorization policy for a set of roles. These are used via AuthorizeAttributes. A user with any one
    /// of the allowed roles will be authorized for the policy. For example, a role can be an AD group name. Having
    /// any role within the policy will grant access.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="authorizationSettings">Authorization settings from configuration</param>
    public static void AddAuthorizationPoliciesFromSettings(this IServiceCollection services, AuthorizationSettings authorizationSettings)
    {
        authorizationSettings
            .EnsureNotNull(nameof(authorizationSettings))
            .Policies
                .EnsureNotNullOrEmpty(nameof(authorizationSettings), "Authorization Policies not found in application configuration.");

        services.AddAuthorization(options =>
        {
            foreach (var (key, value) in authorizationSettings.Policies)
            {
                options.AddPolicy(key, builder => builder.RequireRole(value));
            }
        });
    }

    /// <summary>
    /// Add filter to every endpoint for authorization.
    /// </summary>
    /// <param name="services">This service collection</param>
    /// <param name="policy">The policy name</param>
    public static void AddGlobalAuthorizeFilter(this IServiceCollection services, string policy)
    {
        services.AddControllers(options => options.Filters.Add(new AuthorizeFilter(policy)));
    }

    /// <summary>
    /// Add a singleton accessor for the current web user.
    /// </summary>
    /// <param name="services">This service collection</param>
    public static void AddWebCurrentUserAccessor(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserAccessor, WebCurrentUserAccessor>();
    }

    /// <summary>
    /// Add Windows Authentication. Used in Microsoft enterprise environments with Active Directory authentication.
    /// Adds an AllowAnonymousFilter when in Development for Kestrel which essentially disables all auth. Use Staging
    /// environment to test authorization.
    /// </summary>
    /// <param name="services">This service collection</param>
    /// <param name="environment">The hosting environment</param>
    public static void AddWindowsAuthentication(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);

        if (environment.IsDevelopment())
        {
            services.AddControllers(options => options.Filters.Add(new AllowAnonymousFilter()));
        }
    }
}
