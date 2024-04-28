using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace VoidCore.AspNet.Routing;

/// <summary>
/// Configuration for SPA routing and exception handling.
/// </summary>
public static class RoutingServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="ApiRouteExceptionFilterAttribute"/> globally.
    /// </summary>
    /// <param name="services">The services collection</param>
    public static void AddApiExceptionFilter(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(new TypeFilterAttribute(typeof(ApiRouteExceptionFilterAttribute)));
        });
    }
}
