using Microsoft.AspNetCore.Mvc;

namespace VoidCore.AspNet.Attributes
{
    /// <summary>
    /// An attribute that conventionalizes the routes to api endpoints at "~/api".
    /// </summary>
    public class ApiRouteAttribute : RouteAttribute
    {
        /// <summary>
        /// The base path to all api endpoints.
        /// </summary>
        public static string BasePath => "/api";

        /// <summary>
        /// Construct a new ApiRoute.
        /// </summary>
        /// <param name="path">The endpoint route to be appended to the basepath. Typically the name of the REST entity</param>
        /// <returns></returns>
        public ApiRouteAttribute(string path) : base($"{BasePath}/{path}") { }
    }
}
