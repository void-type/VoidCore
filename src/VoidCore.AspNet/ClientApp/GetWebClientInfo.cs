using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Auth;
using VoidCore.Model.Configuration;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses;

// Allow single file events
#pragma warning disable CA1034

// Allow single file events
#pragma warning disable CA1034

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// A domain event group for getting information to bootstrap a web SPA client.
    /// </summary>
    public static class GetWebClientInfo
    {
        /// <inheritdoc/>
        public class Handler : EventHandlerSyncAbstract<Request, WebClientInfo>
        {
            private readonly IAntiforgery _antiForgery;
            private readonly IWebAppVariables _webAppVariables;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IHttpContextAccessor _httpContextAccessor;

            /// <summary>
            /// Construct a new handler for GetWebClientInfo
            /// </summary>
            /// <param name="webAppVariables">Application settings pulled from configuration</param>
            /// <param name="httpContextAccessor">Accessor for the current httpContext</param>
            /// <param name="antiforgery">The ASP.NET antiForgery object</param>
            /// <param name="currentUserAccessor">UI-friendly user name</param>
            public Handler(IWebAppVariables webAppVariables, IHttpContextAccessor httpContextAccessor, IAntiforgery antiforgery, ICurrentUserAccessor currentUserAccessor)
            {
                _webAppVariables = webAppVariables;
                _httpContextAccessor = httpContextAccessor;
                _antiForgery = antiforgery;
                _currentUserAccessor = currentUserAccessor;
            }

            /// <inheritdoc/>
            protected override IResult<WebClientInfo> HandleSync(Request request)
            {
                var clientInfo = new WebClientInfo(
                    _webAppVariables.AppName,
                    _antiForgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).RequestToken,
                    _antiForgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).HeaderName,
                    _currentUserAccessor);

                return Ok(clientInfo);
            }
        }

        /// <summary>
        /// Request for GetWebClientInfo handler
        /// </summary>
        public class Request { }

        /// <summary>
        /// Information for bootstrapping a web client.
        /// </summary>
        public class WebClientInfo
        {
            internal WebClientInfo(string applicationName, string antiforgeryToken, string antiforgeryTokenHeaderName, ICurrentUserAccessor currentUserAccessor)
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
        /// Log the GetWebClientInfo result.
        /// </summary>
        public class Logger : FallibleEventLogger<Request, WebClientInfo>
        {
            /// <inheritdoc/>
            public Logger(ILoggingService logger) : base(logger) { }

            /// <summary>
            /// Overrides the base OnSuccess to log some information about the resultant application information.
            /// </summary>
            /// <param name="request">The request of the event</param>
            /// <param name="response">The successful result of the event</param>
            protected override void OnSuccess(Request request, WebClientInfo response)
            {
                Logger.Info(
                    $"AppName: {response.ApplicationName}",
                    $"UserName: {response.User.Login}",
                    $"UserAuthorizedAs: {string.Join(", ", response.User.AuthorizedAs)}");

                base.OnSuccess(request, response);
            }
        }
    }
}

#pragma warning restore CA1034
