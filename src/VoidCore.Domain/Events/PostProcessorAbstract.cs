namespace VoidCore.Domain.Events
{
    /// <summary>
    /// A post processor with separate channels for successful events, failed events, and both.
    /// </summary>
    /// <typeparam name="TRequest">The request of the event</typeparam>
    /// <typeparam name="TResponse">The response of the event</typeparam>
    public abstract class PostProcessorAbstract<TRequest, TResponse> : IPostProcessor<TRequest, TResponse>
    {
        /// <inheritdoc/>
        public void Process(TRequest request, IResult<TResponse> result)
        {
            OnBoth(request, result);

            if (result.IsSuccess)
            {
                OnSuccess(request, result.Value);
            }
            else
            {
                OnFailure(request, result);
            }
        }

        /// <summary>
        /// Override this method to process regardless of success or failure.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="result">The result of the event, this contains the response if successful</param>
        protected virtual void OnBoth(TRequest request, IResult<TResponse> result) { }

        /// <summary>
        /// Override this method to process after a validation or event failure.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="result">The failed result of the event</param>
        protected virtual void OnFailure(TRequest request, IResult result) { }

        /// <summary>
        /// Override this method to process on success of the event.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="response">The result value of the successful event</param>
        protected virtual void OnSuccess(TRequest request, TResponse response) { }
    }
}
