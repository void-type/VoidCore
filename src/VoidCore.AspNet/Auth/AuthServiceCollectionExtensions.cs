using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.Domain.Guards;

namespace VoidCore.AspNet.Auth
{
    /// <summary>
    /// Configuration for authentication and authorization.
    /// </summary>
    public static class AuthorizationServiceCollectionExtensions
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
            authorizationSettings?.Policies
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
            services.AddMvc(opt => opt.Filters.Add(new AuthorizeFilter(policy)));
        }

        /// <summary>
        /// Add Windows Authentication. Used in Microsoft enterprise environments with Active Directory authentication.
        /// Adds an AllowAnonymousFilter when in Development for Kestrel which essentially disables all auth. Use Staging
        /// environment to test authorization.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="environment">The hosting environment</param>
        public static void AddWindowsAuthentication(this IServiceCollection services, IHostingEnvironment environment)
        {
            services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);

            if (environment.IsDevelopment())
            {
                services.AddMvc(options => { options.Filters.Add(new AllowAnonymousFilter()); });
            }
        }
    }
}
