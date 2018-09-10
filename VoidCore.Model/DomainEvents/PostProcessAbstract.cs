namespace VoidCore.Model.DomainEvents
{
    /// <inheritdoc/>
    public abstract class PostProcessorAbstract<TRequest, TResponse> : IPostProcessor<TRequest, TResponse>
    {
        /// <inheritdoc/>
        public void Process(TRequest request, Result<TResponse> result)
        {
            OnBoth(request, result);

            if (result.IsSuccess)
            {
                OnSuccess(request, result);
            }
            else
            {
                OnFailure(request, result);
            }
        }

        /// <summary>
        /// Overridee this method to process regardless of success or failure.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="result">The result of the event, this contains the response if successful</param>
        public virtual void OnBoth(TRequest request, Result<TResponse> result) { }

        /// <summary>
        /// Override this method to process after a validation or event failure.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="result">The result of the event, this contains the response if successful</param>
        public virtual void OnFailure(TRequest request, Result<TResponse> result) { }

        /// <summary>
        /// Override this method to process on success of the event.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="result">The result of the event, this contains the response if successful</param>
        public virtual void OnSuccess(TRequest request, Result<TResponse> result) { }
    }
}
