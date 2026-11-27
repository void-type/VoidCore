using Microsoft.AspNetCore.Http;

namespace VoidCore.AspNet.Routing;

/// <summary>
/// The shared definition of where API endpoints live. Controllers should route under <see cref="BasePath"/> so that they agree with
/// the exception handling and SPA fallback in <see cref="RoutingApplicationBuilderExtensions"/>, for example
/// <c>[Route(ApiRoutes.BasePath + "/recipes")]</c>.
/// </summary>
public static class ApiRoutes
{
    /// <summary>
    /// The base path to all API endpoints.
    /// </summary>
    public const string BasePath = "/api";

    /// <summary>
    /// Check if the request path is BasePath or starts with it as a path segment ("/api/x" but not "/apiary").
    /// </summary>
    /// <param name="context">The HTTP context to check</param>
    public static bool IsApiRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments(BasePath, StringComparison.OrdinalIgnoreCase);
    }
}
