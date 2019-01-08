using System.Threading;
using System.Threading.Tasks;

namespace VoidCore.Domain.Events
{
    /// <summary>
    /// An event in the domain that asynchronously takes a request and returns a response. The event request can be validated before handling. The
    /// event can also be fallible, returning a Result of response. The event can be appended with post processors for concerns like logging.
    /// </summary>
    /// <typeparam name="TRequest">The type of the event request</typeparam>
    /// <typeparam name="TResponse">The type of the event response</typeparam>
    public interface IEventHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Add a post processor to run after the event has been handled.
        /// </summary>
        /// <param name="processor">The IPostProcessor</param>
        /// <returns>The event for chaining setup commands</returns>
        EventHandlerDecorator<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor);

        /// <summary>
        /// Add a validator to validate the request. All validators are run before checking results.
        /// </summary>
        /// <param name="validator">The IValidator</param>
        /// <returns>The event for chaining setup commands</returns>
        EventHandlerDecorator<TRequest, TResponse> AddRequestValidator(IRequestValidator<TRequest> validator);

        /// <summary>
        /// Handle the domain event. This includes validating the request, handling the event, and running any post processors.
        /// </summary>
        /// <param name="request">The request contains all the parameters to handle the event.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        /// <returns>A result of the response.</returns>
        Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
