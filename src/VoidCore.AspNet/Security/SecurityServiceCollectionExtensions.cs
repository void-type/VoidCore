using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Configuration for security.
    /// </summary>
    public static class SecurityServiceCollectionExtensions
    {
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
        /// Setup HttpsRedirection with a port if not already set. Port will be detected from the ASPNETCORE_HTTPS_PORT env var, sslPort or Https URL
        /// in launchSettings.json. If no port is detected, this method will use the override supplied or 443.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="environment">The hosting environment</param>
        /// <param name="httpsPortOverride">The HTTPS port to override anything configured before this point</param>
        public static void AddSecureTransport(this IServiceCollection services, IHostingEnvironment environment, int httpsPortOverride = 443)
        {
            if (!environment.IsDevelopment())
            {
                services.AddHsts(options =>
                {
                    options.MaxAge = TimeSpan.FromDays(365);
                    options.IncludeSubDomains = true;
                });
            }

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