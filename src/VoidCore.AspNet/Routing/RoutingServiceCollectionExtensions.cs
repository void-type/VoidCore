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
        public static void AddApiExceptionFilter(this IServiceCollection services)
        {
            static void Config(MvcOptions options)
            {
                options.Filters.Add(new TypeFilterAttribute(typeof(ApiRouteExceptionFilterAttribute)));
            }

#if NETCOREAPP3_0
            services.AddControllers(Config);
#else
            services.AddMvc(Config);
#endif
        }
    }
}
