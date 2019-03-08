namespace VoidCore.Domain.Events
{
    /// <summary>
    /// A device for processing after the domain event is tried. Processors run regardless if the domain event was
    /// handled or if validation failed. Post processing has access to the request and response and therefore should
    /// respect immutability of the request and response. Post processing should also not block the main thread and
    /// should be considered "fire and forget."
    /// </summary>
    /// <typeparam name="TRequest">The request type of the event.</typeparam>
    /// <typeparam name="TResponse">The response type of the event.</typeparam>
    public interface IPostProcessor<in TRequest, TResponse>
    {
        /// <summary>
        /// Process the request and result of the event.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="result">The result of the event, this contains the response if successful</param>
        void Process(TRequest request, IResult<TResponse> result);
    }
}
