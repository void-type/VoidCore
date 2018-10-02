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
    public class EventHandlerDecorator<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Create a new Decorated Domain Event
        /// </summary>
        /// <param name="innerEvent"></param>
        public EventHandlerDecorator(EventHandlerAbstract<TRequest, TResponse> innerEvent)
        {
            _innerEvent = innerEvent;
        }

        /// <inheritdoc/>
        public Result<TResponse> Handle(TRequest request)
        {
            var validation = _validators
                .Select(validator => validator.Validate(request))
                .Combine();

            var result = validation.IsSuccess ?
                _innerEvent.Handle(request) :
                Result.Fail<TResponse>(validation.Failures);

            foreach (var postProcessor in _postProcessors)
            {
                postProcessor.Process(request, result);
            }

            return result;
        }

        /// <inheritdoc/>
        public EventHandlerDecorator<TRequest, TResponse> AddRequestValidator(IValidator<TRequest> validator)
        {
            _validators.Add(validator);
            return this;
        }

        /// <inheritdoc/>
        public EventHandlerDecorator<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
        {
            _postProcessors.Add(processor);
            return this;
        }

        private readonly EventHandlerAbstract<TRequest, TResponse> _innerEvent;
        private readonly List<IValidator<TRequest>> _validators = new List<IValidator<TRequest>>();
        private readonly List<IPostProcessor<TRequest, TResponse>> _postProcessors = new List<IPostProcessor<TRequest, TResponse>>();
    }
}
