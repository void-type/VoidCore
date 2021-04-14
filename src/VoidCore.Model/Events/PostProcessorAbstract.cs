using System.Collections.Generic;
using VoidCore.Model.Functional;

namespace VoidCore.Model.Events
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
            result
                .Tee(result => OnBoth(request, result))
                .TeeOnSuccess(response => OnSuccess(request, response))
                .TeeOnFailure(failures => OnFailure(request, failures));
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
        /// <param name="failures">The failures of the result of the event</param>
        protected virtual void OnFailure(TRequest request, IEnumerable<IFailure> failures) { }

        /// <summary>
        /// Override this method to process on success of the event.
        /// </summary>
        /// <param name="request">The domain event request</param>
        /// <param name="response">The result value of the successful event</param>
        protected virtual void OnSuccess(TRequest request, TResponse response) { }
    }
}
