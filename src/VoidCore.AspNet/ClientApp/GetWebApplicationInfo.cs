using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Auth;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// A domain event group for getting information to boostrap a web SPA client.
    /// </summary>
    public class GetWebApplicationInfo
    {
        /// <inheritdoc/>
        public class Handler : EventHandlerSyncAbstract<Request, WebApplicationInfo>
        {
            private readonly IAntiforgery _antiforgery;
            private readonly ApplicationSettings _applicationSettings;
            private readonly ICurrentUserAccessor _currentUser;
            private readonly IHttpContextAccessor _httpContextAccessor;

            /// <summary>
            /// Construct a new handler for GetApplicationInfo
            /// </summary>
            /// <param name="applicationSettings">Application settings pulled from configuration</param>
            /// <param name="httpContextAccessor">Accessor for the current httpcontext</param>
            /// <param name="antiforgery">The ASP.NET antiforgery object</param>
            /// <param name="currentUser">UI-friendly user name</param>
            public Handler(ApplicationSettings applicationSettings, IHttpContextAccessor httpContextAccessor, IAntiforgery antiforgery, ICurrentUserAccessor currentUser)
            {
                _applicationSettings = applicationSettings;
                _httpContextAccessor = httpContextAccessor;
                _antiforgery = antiforgery;
                _currentUser = currentUser;
            }

            /// <inheritdoc/>
            protected override IResult<WebApplicationInfo> HandleSync(Request request)
            {
                var applicationInfo = new WebApplicationInfo(
                    _applicationSettings.Name,
                    _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).RequestToken,
                    _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).HeaderName,
                    _currentUser);

                return Result.Ok(applicationInfo);
            }
        }

        /// <summary>
        /// Request for GetApplicationInfo handler
        /// </summary>
        public class Request { }

        /// <summary>
        /// Information for bootstrapping a web client.
        /// </summary>
        public class WebApplicationInfo
        {
            internal WebApplicationInfo(string applicationName, string antiforgeryToken, string antiforgeryTokenHeaderName, ICurrentUserAccessor currentUserAccessor)
            {
                ApplicationName = applicationName;
                AntiforgeryToken = antiforgeryToken;
                AntiforgeryTokenHeaderName = antiforgeryTokenHeaderName;
                User = currentUserAccessor.User;
            }

            /// <summary>
            /// The value of the header antiforgery token
            /// </summary>
            public string AntiforgeryToken { get; }

            /// <summary>
            /// The header name of the antiforgery token
            /// </summary>
            public string AntiforgeryTokenHeaderName { get; }

            /// <summary>
            /// The UI-friendly application name.
            /// </summary>
            public string ApplicationName { get; }

            /// <summary>
            /// The current user
            /// </summary>
            public DomainUser User { get; }
        }

        /// <summary>
        /// Log the GetApplicationInfo result.
        /// </summary>
        public class Logger : FallibleEventLogger<Request, WebApplicationInfo>
        {
            /// <inheritdoc/>
            public Logger(ILoggingService logger) : base(logger) { }

            /// <summary>
            /// Overrides the base OnSuccess to log some information about the resultant application information.
            /// </summary>
            /// <param name="request">The request of the event</param>
            /// <param name="response">The successful result of the event</param>
            protected override void OnSuccess(Request request, WebApplicationInfo response)
            {
                Logger.Info(
                    $"AppName: {response.ApplicationName}",
                    $"UserName: {response.User.Name}",
                    $"UserAuthorizedAs: {string.Join(", ", response.User.AuthorizedAs)}");

                base.OnSuccess(request, response);
            }
        }
    }
}
