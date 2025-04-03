using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VoidCore.AspNet.Routing;

/// <summary>
/// Prepend the API route base of "/api" to the route path.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ApiRouteAttribute : RouteAttribute
{
    /// <summary>
    /// The base path to all API endpoints.
    /// </summary>
    public const string BasePath = "/api";

    /// <summary>
    /// Construct a new ApiRoute.
    /// </summary>
    /// <param name="path">The endpoint route to be appended to the BasePath. Typically the name of the REST entity</param>
    public ApiRouteAttribute(string path) : base($"{BasePath}/{path}") { }

    /// <summary>
    /// Check if the request path starts with BasePath.
    /// </summary>
    public static bool IsApiRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments(BasePath, StringComparison.OrdinalIgnoreCase);
    }
}
