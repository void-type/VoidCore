using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VoidCore.Domain.Events
{
    /// <summary>
    /// A wrapper that decorates a domain event with validators and post processors.
    /// </summary>
    /// <typeparam name="TRequest">The type of the event request</typeparam>
    /// <typeparam name="TResponse">The type of the event response</typeparam>
    public class EventHandlerDecorator<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Create a new Decorated Domain Event handler
        /// </summary>
        /// <param name="innerEvent">The inner domain handler</param>
        public EventHandlerDecorator(EventHandlerAbstract<TRequest, TResponse> innerEvent)
        {
            _innerEvent = innerEvent;
        }

        /// <inheritdoc/>
        public EventHandlerDecorator<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
        {
            _postProcessors.Add(processor);
            return this;
        }

        /// <inheritdoc/>
        public EventHandlerDecorator<TRequest, TResponse> AddRequestValidator(IRequestValidator<TRequest> validator)
        {
            _requestValidators.Add(validator);
            return this;
        }

        /// <inheritdoc/>
        public async Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _requestValidators
                .Select(validator => validator.Validate(request))
                .Combine()
                .ThenAsync(() => _innerEvent.Handle(request, cancellationToken));

            foreach (var postProcessor in _postProcessors)
            {
                postProcessor.Process(request, result);
            }

            return result;
        }

        private readonly EventHandlerAbstract<TRequest, TResponse> _innerEvent;
        private readonly List<IPostProcessor<TRequest, TResponse>> _postProcessors = new List<IPostProcessor<TRequest, TResponse>>();
        private readonly List<IRequestValidator<TRequest>> _requestValidators = new List<IRequestValidator<TRequest>>();
    }
}
