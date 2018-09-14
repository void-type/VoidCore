using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Exceptions;

namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// Extensions to the ServiceCollection to setup application dependencies.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Setup an authorization policy for a set of roles. These are used via AuthorizeAttributes.
        /// A user with any one of the allowed roles will be authorized for the policy.
        /// For example, a role can be an AD group name. Having any role within the policy will grant access.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="authorizationSettings">Authorization settings from configuration</param>
        public static void AddAuthorizationPoliciesFromSettings(this IServiceCollection services, AuthorizationSettings authorizationSettings)
        {
            services.AddAuthorization(options => authorizationSettings.Policies
                .ToList()
                .ForEach(policy => options
                    .AddPolicy(policy.Key, p => p
                        .RequireRole(policy.Value))));
        }

        /// <summary>
        /// Add a global filter for handling uncaught API exceptions.
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="environment">The hosting environment</param>
        public static void AddApiExceptionFilter(this IServiceCollection services, IHostingEnvironment environment)
        {
            services.AddMvc(options => options.Filters.Add(new TypeFilterAttribute(typeof(ApiExceptionFilterAttribute))));
        }

        /// <summary>
        /// Add filter to every endpoint for authorization.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="policy">The policy name</param>
        public static void AddGlobalAuthorizeFilter(this IServiceCollection services, string policy)
        {
            services.AddMvc(o => o.Filters.Add(new AuthorizeFilter(policy)));
        }

        /// <summary>
        /// Setup Antiforgery token and filters for all non-GET requests.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="environment">The hosting environment</param>
        public static void AddAntiforgery(this IServiceCollection services, IHostingEnvironment environment)
        {
            services.AddMvc(options => { options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); });
            services.AddAntiforgery(options => { options.HeaderName = "X-Csrf-Token"; });
        }

        /// <summary>
        /// Pulls a settings object from configuration and adds it as a singleton to the DI container.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.null</returns>
        public static TSettings AddSettingsSingleton<TSettings>(this IServiceCollection services, IConfiguration configuration) where TSettings : class, new()
        {
            var settings = new TSettings();
            configuration.Bind(settings);
            services.AddSingleton(settings);
            return settings;
        }

        /// <summary>
        /// Add a DbContext to the DI container using SQL server settings.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="connectionString">The connection string to send to the dbcontext</param>
        /// <typeparam name="TDbContext">The concrete type of DbContext to add to the DI container</typeparam>
        public static void AddSqlServerDbContext<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Add Windows Authentication. Used in Microsoft enterprise environments with Active Directory authentication.
        /// Adds an AllowAnonymousFilter when in Development for Kestrel which essentially disables all auth.
        /// Use Staging to test authorization.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="environment">The hosting environment</param>
        public static void AddWindowsAuthentication(this IServiceCollection services, IHostingEnvironment environment)
        {
            // Get Windows Authentication from IIS
            services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);

            if (environment.IsDevelopment())
            {
                // Disable authentication in development to run in Kestrel.
                services.AddMvc(options => { options.Filters.Add(new AllowAnonymousFilter()); });
            }
        }
    }
}
