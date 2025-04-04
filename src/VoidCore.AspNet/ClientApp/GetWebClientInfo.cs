﻿using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VoidCore.AspNet.Configuration;
using VoidCore.Model.Auth;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;
using VoidCore.Model.Responses;

// Allow single file events
#pragma warning disable CA1034

namespace VoidCore.AspNet.ClientApp;

/// <summary>
/// A domain event group for getting information to bootstrap a web SPA client.
/// </summary>
public static class GetWebClientInfo
{
    /// <inheritdoc/>
    public class Handler : EventHandlerAbstract<Request, WebClientInfo>
    {
        private readonly IAntiforgery _antiForgery;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly WebApplicationSettings _applicationSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Construct a new handler for GetWebClientInfo.
        /// </summary>
        /// <param name="applicationSettings">Application settings pulled from configuration</param>
        /// <param name="httpContextAccessor">Accessor for the current httpContext</param>
        /// <param name="antiforgery">The ASP.NET antiForgery object</param>
        /// <param name="currentUserAccessor">UI-friendly user name</param>
        public Handler(WebApplicationSettings applicationSettings, IHttpContextAccessor httpContextAccessor, IAntiforgery antiforgery, ICurrentUserAccessor currentUserAccessor)
        {
            _applicationSettings = applicationSettings;
            _httpContextAccessor = httpContextAccessor;
            _antiForgery = antiforgery;
            _currentUserAccessor = currentUserAccessor;
        }

        /// <inheritdoc/>
        public override async Task<IResult<WebClientInfo>> Handle(Request request, CancellationToken cancellationToken = default)
        {
            var context = _httpContextAccessor.HttpContext.EnsureNotNull();

            var user = await _currentUserAccessor.GetUser();

            var tokens = _antiForgery.GetAndStoreTokens(context);

            var clientInfo = new WebClientInfo(
                _applicationSettings.Name,
                tokens.RequestToken.EnsureNotNull(),
                tokens.HeaderName.EnsureNotNull(),
                user);

            return Ok(clientInfo);
        }
    }

#pragma warning disable S2094
    /// <summary>
    /// Request for GetWebClientInfo handler
    /// </summary>
    public class Request { }
#pragma warning restore S2094

    /// <summary>
    /// Information for bootstrapping a web client.
    /// </summary>
    public class WebClientInfo
    {
        internal WebClientInfo(string applicationName, string antiforgeryToken, string antiforgeryTokenHeaderName, DomainUser user)
        {
            ApplicationName = applicationName;
            AntiforgeryToken = antiforgeryToken;
            AntiforgeryTokenHeaderName = antiforgeryTokenHeaderName;
            User = user;
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
    /// Log the GetWebClientInfo response.
    /// </summary>
    public class ResponseLogger : FallibleEventLoggerAbstract<Request, WebClientInfo>
    {
        /// <inheritdoc cref="FallibleEventLoggerAbstract{TRequest, TResponse}"/>
        public ResponseLogger(ILogger<ResponseLogger> logger) : base(logger) { }

        /// <inheritdoc/>
        protected override void OnSuccess(Request request, WebClientInfo response)
        {
            Logger.LogInformation("Responded with {ResponseType}. AppName: {ApplicationName} UserName: {UserName} UserAuthorizedAs: {AuthorizedAs}",
                nameof(WebClientInfo),
                response.ApplicationName,
                response.User.Login,
                string.Join(", ", response.User.AuthorizedAs));

            base.OnSuccess(request, response);
        }
    }

    /// <summary>
    /// The pipeline for this event.
    /// </summary>
    public class Pipeline : EventPipelineAbstract<Request, WebClientInfo>
    {
        /// <summary>
        /// Use DI to construct the pipeline.
        /// </summary>
        /// <param name="handler">The event handler</param>
        /// <param name="responseLogger">The response logger</param>
        public Pipeline(Handler handler, ResponseLogger responseLogger)
        {
            InnerHandler = handler
                .AddPostProcessor(responseLogger);
        }
    }
}
