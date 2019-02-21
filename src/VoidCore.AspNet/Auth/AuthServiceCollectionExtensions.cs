using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace VoidCore.AspNet.Auth
{
    /// <summary>
    /// Configuration for authentication and authorization.
    /// </summary>
    public static class AuthorizationServiceCollectionExtensions
    {
        /// <summary>
        /// Setup an authorization policy for a set of roles. These are used via AuthorizeAttributes. A user with any one of the allowed roles will be
        /// authorized for the policy. For example, a role can be an AD group name. Having any role within the policy will grant access.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="authorizationSettings">Authorization settings from configuration</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if authorizationSettings are not configured.</exception>
        /// <exception cref="ArgumentException">Throws an ArgumentException if authorizationPolicies are not configured.</exception>
        public static void AddAuthorizationPoliciesFromSettings(this IServiceCollection services, AuthorizationSettings authorizationSettings)
        {
            if (authorizationSettings == null)
            {
                throw new ArgumentNullException(nameof(authorizationSettings), "Application is not configured properly. AuthorizationSettings not found.");
            }

            if (authorizationSettings.Policies == null || !authorizationSettings.Policies.Any())
            {
                throw new ArgumentException("Application is not properly configured. AuthorizationPolicies is either empty or not found.", nameof(authorizationSettings));
            }

            services.AddAuthorization(options =>
            {
                foreach (var policy in authorizationSettings.Policies)
                {
                    options.AddPolicy(policy.Key, builder => builder.RequireRole(policy.Value));
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
        /// Adds an AllowAnonymousFilter when in Development for Kestrel which essentially disables all auth.
        /// Use Staging environment to test authorization.
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
