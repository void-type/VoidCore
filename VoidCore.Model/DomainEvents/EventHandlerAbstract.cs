using VoidCore.Model.Validation;

namespace VoidCore.Model.DomainEvents
{
    /// <inheritdoc/>
    public abstract class EventHandlerAbstract<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    {
        /// <inheritdoc/>
        public Result<TResponse> Handle(TRequest request)
        {
            return HandleInternal(request);
        }

        /// <inheritdoc/>
        public EventHandlerDecorator<TRequest, TResponse> AddRequestValidator(IValidator<TRequest> validator)
        {
            return new EventHandlerDecorator<TRequest, TResponse>(this)
                .AddRequestValidator(validator);
        }

        /// <inheritdoc/>
        public EventHandlerDecorator<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
        {
            return new EventHandlerDecorator<TRequest, TResponse>(this)
                .AddPostProcessor(processor);
        }

        /// <summary>
        /// Override this method to provide domain logic to handle the validated request and return an appropriate result of the response.
        /// Cross-cutting concerns or non-domain elements should be put into post processors.
        /// </summary>
        /// <param name="request">The validated request</param>
        /// <returns>A result of TResponse</returns>
        protected abstract Result<TResponse> HandleInternal(TRequest request);
    }
}
