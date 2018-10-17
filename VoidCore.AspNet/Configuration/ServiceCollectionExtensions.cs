using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using VoidCore.AspNet.Exceptions;
using VoidCore.Model.ClientApp;

namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// Extensions to the ServiceCollection to setup application dependencies.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Pulls a settings object from configuration and adds it as a singleton to the DI container.
        /// Uses naming conventions of the Settings class to find the config section.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <param name="root">When true, the naming conventions are ignored and settings are assumed to be at the root of the config</param>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.</returns>
        public static TSettings AddSettingsSingleton<TSettings>(this IServiceCollection services, IConfiguration configuration, bool root = false)
        where TSettings : class, new()
        {
            if (!root)
            {
                var sectionName = ConfigHelpers.StripEndingFromType(typeof(TSettings), "settings");
                configuration = configuration.GetSection(sectionName);
            }

            var settings = configuration.Get<TSettings>(options => options.BindNonPublicProperties = true);
            services.AddSingleton(settings);
            return settings;
        }

        /// <summary>
        /// Pulls a settings object from configuration and adds it as a singleton to the DI container via an interface.
        /// Uses naming conventions of the Settings class to find the config section.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <param name="root">When true, the naming conventions are ignored and settings are assumed to be at the root of the config</param>
        /// <typeparam name="TService">An interface or higher-level service to access the settings from</typeparam>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.</returns>
        public static TSettings AddSettingsSingleton<TService, TSettings>(this IServiceCollection services, IConfiguration configuration, bool root = false)
        where TSettings : class, TService, new()
        where TService : class
        {
            if (!root)
            {
                var sectionName = ConfigHelpers.StripEndingFromType(typeof(TSettings), "settings");
                configuration = configuration.GetSection(sectionName);
            }

            var settings = configuration.Get<TSettings>(options => options.BindNonPublicProperties = true);
            services.AddSingleton<TService>(x => settings);
            return settings;
        }

        /// <summary>
        /// Add a DbContext to the DI container using SQL server settings.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="connectionString">The connection string to send to the DbContext</param>
        /// <typeparam name="TDbContext">The concrete type of DbContext to add to the DI container</typeparam>
        public static void AddSqlServerDbContext<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Application is not properly configured. Connection string is either empty or not found.");
            }

            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
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
            // Get Windows Authentication from IIS
            services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);

            if (environment.IsDevelopment())
            {
                // Disable authentication in development to run in Kestrel.
                services.AddMvc(options => { options.Filters.Add(new AllowAnonymousFilter()); });
            }
        }

        /// <summary>
        /// Setup an authorization policy for a set of roles. These are used via AuthorizeAttributes.
        /// A user with any one of the allowed roles will be authorized for the policy.
        /// For example, a role can be an AD group name. Having any role within the policy will grant access.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="applicationSettings">Authorization settings from configuration</param>
        public static void AddAuthorizationPoliciesFromSettings(this IServiceCollection services, IApplicationSettings applicationSettings)
        {
            if (applicationSettings?.AuthorizationPolicies == null || !applicationSettings.AuthorizationPolicies.Any())
            {
                throw new ArgumentNullException(nameof(applicationSettings), "Application is not properly configured. AuthorizationPolicies is either empty or not found.");
            }

            services.AddAuthorization(options =>
            {
                foreach (var policy in applicationSettings.AuthorizationPolicies)
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
        /// Add a global filter for handling uncaught API exceptions.
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="environment">The hosting environment</param>
        public static void AddApiExceptionFilter(this IServiceCollection services, IHostingEnvironment environment)
        {
            services.AddMvc(options => options.Filters.Add(new TypeFilterAttribute(typeof(ApiExceptionFilterAttribute))));
        }

        /// <summary>
        /// Setup Antiforgery token and filters for all non-GET requests.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="environment">The hosting environment</param>
        public static void AddAntiforgery(this IServiceCollection services, IHostingEnvironment environment)
        {
            services.AddMvc(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            services.AddAntiforgery(options => { options.HeaderName = "X-Csrf-Token"; });
        }

        /// <summary>
        /// Setup HttpsRedirection with a port if not already set. Port will be detected from the ASPNETCORE_HTTPS_PORT env var,
        /// sslPort or Https URL in launchSettings.json. If no port is detected, this method will use the override supplied or 443.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="httpsPortOverride"></param>
        public static void AddSecureTransport(this IServiceCollection services, int httpsPortOverride = 443)
        {
            services.AddHttpsRedirection(options =>
            {
                if (options.HttpsPort == null)
                {
                    options.HttpsPort = httpsPortOverride;
                }
            });
        }
    }
}
