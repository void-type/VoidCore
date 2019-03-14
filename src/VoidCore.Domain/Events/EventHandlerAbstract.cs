using System.Threading;
using System.Threading.Tasks;

namespace VoidCore.Domain.Events
{
    /// <inheritdoc/>
    public abstract class EventHandlerAbstract<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    {
        /// <inheritdoc/>
        public EventHandlerDecorator<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
        {
            return new EventHandlerDecorator<TRequest, TResponse>(this)
                .AddPostProcessor(processor);
        }

        /// <inheritdoc/>
        public EventHandlerDecorator<TRequest, TResponse> AddRequestValidator(IRequestValidator<TRequest> validator)
        {
            return new EventHandlerDecorator<TRequest, TResponse>(this)
                .AddRequestValidator(validator);
        }

        /// <summary>
        /// Override this method to provide domain logic to handle the validated request and return an appropriate result
        /// of the response.
        /// </summary>
        /// <param name="request">The validated request</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        /// <returns>A Task of Result of TResponse</returns>
        public abstract Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default);
    }
}
