using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Validation;

namespace VoidCore.Model.DomainEvents
{
    /// <inheritdoc/>
    public abstract class DomainEventAbstract<TRequest, TResponse> : IDomainEvent<TRequest, TResponse>
    {
        /// <inheritdoc/>
        public Result<TResponse> Handle(TRequest request)
        {
            return HandleInternal(request);
        }

        /// <summary>
        /// Add a validator to validate the request. All validators are run before checking results.
        /// </summary>
        /// <param name="validator">The IValidator</param>
        /// <returns>The event for chaining setup commands</returns>
        public DecoratedDomainEvent<TRequest, TResponse> AddRequestValidator(IValidator<TRequest> validator)
        {
            return new DecoratedDomainEvent<TRequest, TResponse>(this)
                .AddRequestValidator(validator);
        }

        /// <summary>
        /// Add a post processor to run after the event has been handled.
        /// </summary>
        /// <param name="processor">The IPostProcessor</param>
        /// <returns>The event for chaining setup commands</returns>
        public DecoratedDomainEvent<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
        {
            return new DecoratedDomainEvent<TRequest, TResponse>(this)
                .AddPostProcessor(processor);
        }

        /// <summary>
        /// Override this method to provide domain logic to handle the validated request and return an appropriate result of the response.
        /// Cross-cutting concerns or non-domain elements should be put into post processors.
        /// </summary>
        /// <param name="request">The validated request</param>
        /// <returns>A result of TResponse</returns>
        protected abstract Result<TResponse> HandleInternal(TRequest request);

        private readonly List<IValidator<TRequest>> _validators = new List<IValidator<TRequest>>();
        private readonly List<IPostProcessor<TRequest, TResponse>> _postProcessors = new List<IPostProcessor<TRequest, TResponse>>();
    }
}
