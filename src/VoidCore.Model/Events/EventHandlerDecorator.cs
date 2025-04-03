using VoidCore.Model.Functional;

namespace VoidCore.Model.Events;

/// <summary>
/// A decorator for building a pipeline from a domain event with validators and post processors.
/// </summary>
/// <typeparam name="TRequest">The type of the event request</typeparam>
/// <typeparam name="TResponse">The type of the event response</typeparam>
public class EventHandlerDecorator<TRequest, TResponse> : IDecoratableEventHandler<TRequest, TResponse>
{
    private readonly EventHandlerAbstract<TRequest, TResponse> _innerEvent;
    private readonly List<IPostProcessor<TRequest, TResponse>> _postProcessors = [];
    private readonly List<IRequestLogger<TRequest>> _requestLoggers = [];
    private readonly List<IRequestValidator<TRequest>> _requestValidators = [];

    /// <summary>
    /// Create a new Decorated Domain Event handler
    /// </summary>
    /// <param name="innerEvent">The inner domain handler</param>
    internal EventHandlerDecorator(EventHandlerAbstract<TRequest, TResponse> innerEvent)
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
    public EventHandlerDecorator<TRequest, TResponse> AddRequestLogger(IRequestLogger<TRequest> logger)
    {
        _requestLoggers.Add(logger);
        return this;
    }

    /// <inheritdoc/>
    public EventHandlerDecorator<TRequest, TResponse> AddRequestValidator(IRequestValidator<TRequest> validator)
    {
        _requestValidators.Add(validator);
        return this;
    }

    /// <inheritdoc/>
    public async Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        foreach (var logger in _requestLoggers)
        {
            logger.Log(request);
        }

        var result = await _requestValidators
            .Select(validator => validator.Validate(request))
            .Combine()
            .ThenAsync(() => _innerEvent.Handle(request, cancellationToken))
            .ConfigureAwait(false);

        foreach (var postProcessor in _postProcessors)
        {
            postProcessor.Process(request, result);
        }

        return result;
    }
}
