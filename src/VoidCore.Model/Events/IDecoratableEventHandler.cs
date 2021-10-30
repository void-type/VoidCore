namespace VoidCore.Model.Events
{
    /// <summary>
    /// An event in the domain that asynchronously takes a request and returns a response.
    /// Events can be fallible, returning a Result of response.
    /// The event request can be validated before handling. The event can be
    /// appended with post processors for concerns like logging.
    /// </summary>
    /// <typeparam name="TRequest">The type of the event request</typeparam>
    /// <typeparam name="TResponse">The type of the event response</typeparam>
    public interface IDecoratableEventHandler<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Add a post processor to run after the event has been handled.
        /// </summary>
        /// <param name="processor">The IPostProcessor</param>
        /// <returns>The event for chaining setup commands</returns>
        EventHandlerDecorator<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor);

        /// <summary>
        /// Add a custom logger to log properties of the request.
        /// </summary>
        /// <param name="logger">The IRequestLogger</param>
        /// <returns>The event for chaining setup commands</returns>
        EventHandlerDecorator<TRequest, TResponse> AddRequestLogger(IRequestLogger<TRequest> logger);

        /// <summary>
        /// Add a validator to validate the request. All validators are run before checking results.
        /// </summary>
        /// <param name="validator">The IValidator</param>
        /// <returns>The event for chaining setup commands</returns>
        EventHandlerDecorator<TRequest, TResponse> AddRequestValidator(IRequestValidator<TRequest> validator);
    }
}
