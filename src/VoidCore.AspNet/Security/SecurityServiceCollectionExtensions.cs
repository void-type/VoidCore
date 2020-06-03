using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Configuration for security.
    /// </summary>
    public static class SecurityServiceCollectionExtensions
    {
        /// <summary>
        /// Setup HttpsRedirection and HSTS for secure transport.
        /// Setup Antiforgery token and filters for all non-GET requests.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="environment">The hosting environment</param>
        public static void AddSpaSecurityServices(this IServiceCollection services, IHostEnvironment environment)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-Csrf-Token";
            });

            if (!environment.IsDevelopment())
            {
                services.AddHsts(options =>
                {
                    options.MaxAge = TimeSpan.FromDays(365);
                    options.IncludeSubDomains = true;
                });
            }
        }
    }
}
