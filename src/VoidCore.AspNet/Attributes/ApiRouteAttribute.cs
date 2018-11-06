using Microsoft.AspNetCore.Mvc;

namespace VoidCore.AspNet.Attributes
{
    /// <summary>
    /// Prepend the API route base of "/api" to the route path.
    /// </summary>
    public class ApiRouteAttribute : RouteAttribute
    {
        /// <summary>
        /// The base path to all API endpoints.
        /// </summary>
        public static string BasePath => "/api";

        /// <summary>
        /// Construct a new ApiRoute.
        /// </summary>
        /// <param name="path">The endpoint route to be appended to the basepath. Typically the name of the REST entity</param>
        public ApiRouteAttribute(string path) : base($"{BasePath}/{path}") { }
    }
}
