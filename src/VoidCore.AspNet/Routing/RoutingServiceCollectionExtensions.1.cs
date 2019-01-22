using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace VoidCore.AspNet.Routing
{
    /// <summary>
    /// Configuration for SPA routing and exception handling.
    /// </summary>
    public static class RoutingServiceCollectionExtensions
    {
        /// <summary>
        /// Add a global filter for handling uncaught API exceptions.
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="environment">The hosting environment</param>
        public static void AddApiExceptionFilter(this IServiceCollection services, IHostingEnvironment environment)
        {
            services.AddMvc(options => options.Filters.Add(new TypeFilterAttribute(typeof(ApiRouteExceptionFilterAttribute))));
        }
    }
}