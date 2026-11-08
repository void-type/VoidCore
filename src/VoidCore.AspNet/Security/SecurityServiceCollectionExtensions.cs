using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VoidCore.AspNet.Security;

/// <summary>
/// Configuration for security.
/// </summary>
public static class SecurityServiceCollectionExtensions
{
    /// <summary>
    /// Setup HttpsRedirection and HSTS for secure transport.
    /// Setup Antiforgery token and filters for all non-GET requests. Validation failures are logged as warnings.
    /// </summary>
    /// <param name="services">This service collection</param>
    /// <param name="environment">The hosting environment</param>
    public static void AddSpaSecurityServices(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

        services.AddAntiforgery(options => options.HeaderName = SecurityConstants.AntiforgeryTokenHeaderName);
        services.AddAntiforgeryFailureLogging();

        if (!environment.IsDevelopment())
        {
            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);
                options.IncludeSubDomains = true;
            });
        }
    }

    private const string AntiforgeryFailureLoggingMarker = "VoidCore.AntiforgeryFailureLogging";

    // The default IAntiforgery is internal, so wrap whatever is registered.
    private static void AddAntiforgeryFailureLogging(this IServiceCollection services)
    {
        // Wrapping twice would log every failure twice.
        if (services.Any(d => d.IsKeyedService && Equals(d.ServiceKey, AntiforgeryFailureLoggingMarker)))
        {
            return;
        }

        services.AddKeyedSingleton<object>(AntiforgeryFailureLoggingMarker, new object());

        var descriptor = services.Last(d => d.ServiceType == typeof(IAntiforgery));
        services.Remove(descriptor);

        services.Add(ServiceDescriptor.Describe(typeof(IAntiforgery), provider =>
        {
            IAntiforgery inner = descriptor switch
            {
                { ImplementationInstance: IAntiforgery instance } => instance,
                { ImplementationFactory: not null } => (IAntiforgery)descriptor.ImplementationFactory(provider),
                _ => (IAntiforgery)ActivatorUtilities.CreateInstance(provider, descriptor.ImplementationType!),
            };

            return new LoggingAntiforgery(inner, provider.GetRequiredService<ILogger<LoggingAntiforgery>>());
        }, descriptor.Lifetime));
    }
}
