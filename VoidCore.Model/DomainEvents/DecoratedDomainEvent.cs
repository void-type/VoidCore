using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Validation;

namespace VoidCore.Model.DomainEvents
{
    /// <summary>
    /// A wrapper that decorates a domain event with validators and post processors.
    /// </summary>
    /// <typeparam name="TRequest">The type of the event request</typeparam>
    /// <typeparam name="TResponse">The type of the event response</typeparam>
    public class DecoratedDomainEvent<TRequest, TResponse> : IDomainEvent<TRequest, TResponse>
    {
        /// <summary>
        /// Create a new Decorated Domain Event
        /// </summary>
        /// <param name="innerEvent"></param>
        public DecoratedDomainEvent(DomainEventAbstract<TRequest, TResponse> innerEvent)
        {
            _innerEvent = innerEvent;
        }

        /// <inheritdoc/>
        public Result<TResponse> Handle(TRequest request)
        {
            var validation = _validators.Select(v => v.Validate(request)).Combine();

            var result = validation.IsSuccess ?
                _innerEvent.Handle(request) :
                Result.Fail<TResponse>(validation.Failures);

            _postProcessors.ForEach(p => p.Process(request, result));

            return result;
        }

        /// <inheritdoc/>
        public DecoratedDomainEvent<TRequest, TResponse> AddRequestValidator(IValidator<TRequest> validator)
        {
            _validators.Add(validator);
            return this;
        }

        /// <inheritdoc/>
        public DecoratedDomainEvent<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
        {
            _postProcessors.Add(processor);
            return this;
        }

        private DomainEventAbstract<TRequest, TResponse> _innerEvent;
        private readonly List<IValidator<TRequest>> _validators = new List<IValidator<TRequest>>();
        private readonly List<IPostProcessor<TRequest, TResponse>> _postProcessors = new List<IPostProcessor<TRequest, TResponse>>();
    }
}
