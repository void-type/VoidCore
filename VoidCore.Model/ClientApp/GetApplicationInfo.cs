using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;

namespace VoidCore.Model.ClientApp
{
    /// <summary>
    /// A domain event group for getting information to boostrap a client application.
    /// </summary>
    public class GetApplicationInfo
    {
        /// <inheritdoc/>
        public class Handler : EventHandlerAbstract<Request, IApplicationInfo>
        {
            /// <summary>
            /// Construct a new handler for GetApplicationInfo
            /// </summary>
            /// <param name="applicationInfo">An instance of the interface IApplicationInfo</param>
            public Handler(IApplicationInfo applicationInfo)
            {
                _applicationInfo = applicationInfo;
            }

            /// <inheritdoc/>
            protected override Result<IApplicationInfo> HandleInternal(Request request)
            {
                return Result.Ok(_applicationInfo);
            }

            private readonly IApplicationInfo _applicationInfo;
        }

        /// <summary>
        /// Request for GetApplicationInfo handler
        /// </summary>
        public class Request { }

        /// <summary>
        /// Information to start the client application
        /// </summary>
        public interface IApplicationInfo
        {
            /// <summary>
            /// The UI-friendly name of the application
            /// </summary>
            /// <value></value>
            string ApplicationName { get; }

            /// <summary>
            /// The UI-friendly user name
            /// </summary>
            /// <value></value>
            ICurrentUser User { get; }
        }

        /// <summary>
        /// Log the GetApplicationInfo result.
        /// </summary>
        public class Logger : FallibleEventLogger<Request, IApplicationInfo>
        {
            /// <inheritdoc/>
            public Logger(ILoggingService logger) : base(logger) { }

            /// <summary>
            /// Override the base OnSuccess to log some information about the resultant IApplicationInfo.
            /// </summary>
            /// <param name="request">The request of the event</param>
            /// <param name="successfulResult">The successful result of the event</param>
            public override void OnSuccess(Request request, IResult<IApplicationInfo> successfulResult)
            {
                Logger.Info(
                    $"AppName: {successfulResult.Value.ApplicationName}",
                    $"UserName: {successfulResult.Value.User.Name}",
                    $"UserAuthorizedAs: {string.Join(", ", successfulResult.Value.User.AuthorizedAs)}");
                base.OnSuccess(request, successfulResult);
            }
        }
    }
}
