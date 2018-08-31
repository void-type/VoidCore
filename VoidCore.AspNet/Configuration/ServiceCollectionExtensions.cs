using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using VoidCore.AspNet.Authorization;

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
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="authorizationSettings">Authorization settings from configuration</param>
        public static void AddAuthorizationPoliciesFromSettings(this IServiceCollection services, AuthorizationSettings authorizationSettings)
        {
            services.AddAuthorization(options => authorizationSettings.Policies.ToList()
                .ForEach(policy => options
                    .AddPolicy(policy.Key, p => p
                        .RequireRole(policy.Value.Select(role => role.Name).ToArray())))
            );
        }

        /// <summary>
        /// Add a global filter for handling uncaught API exceptions.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        public static void AddApiExceptionFilter(this IServiceCollection services, IHostingEnvironment environment)
        {
            services.AddMvc(options => { options.Filters.Add(new TypeFilterAttribute(typeof(ApiExplorerSettingsAttribute))); });
        }

        /// <summary>
        /// Setup Antiforgery token and filters, also sets HTTPS redirection to port 443 by default.
        /// Disables authentication in development.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="environment">The hosting environment</param>
        /// <param name="httpsPort">Override the default https redirect port of 443.</param>
        public static void AddSecureFilters(this IServiceCollection services, IHostingEnvironment environment, int httpsPort = 443)
        {
            if (!environment.IsDevelopment())
            {
                services.AddHttpsRedirection(options => options.HttpsPort = 443);
            }

            // Get the newest MVC behavior.
            // TODO: for aspnet versions newer than 2.1, update or remove this.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Antiforgery on all non-GET requests
            services.AddMvc(options => { options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); });
            services.AddAntiforgery(options => { options.HeaderName = "X-Csrf-Token"; });
        }

        /// <summary>
        /// Pulls a settings object from configuration and adds it as a singleton to the DI container.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
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
        /// Add Windows Authentication. Used in Microsoft Enterprise environments with Active Directory authentication.
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
