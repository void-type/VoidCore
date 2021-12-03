using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Functional;

namespace VoidCore.Model.Events;

/// <inheritdoc/>
public abstract class EventHandlerAbstract<TRequest, TResponse> : IDecoratableEventHandler<TRequest, TResponse>
{
    /// <inheritdoc/>
    public EventHandlerDecorator<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
    {
        return new EventHandlerDecorator<TRequest, TResponse>(this)
            .AddPostProcessor(processor);
    }

    /// <inheritdoc/>
    public EventHandlerDecorator<TRequest, TResponse> AddRequestLogger(IRequestLogger<TRequest> logger)
    {
        return new EventHandlerDecorator<TRequest, TResponse>(this)
            .AddRequestLogger(logger);
    }

    /// <inheritdoc/>
    public EventHandlerDecorator<TRequest, TResponse> AddRequestValidator(IRequestValidator<TRequest> validator)
    {
        return new EventHandlerDecorator<TRequest, TResponse>(this)
            .AddRequestValidator(validator);
    }

    /// <inheritdoc/>
    public abstract Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new successful result for the event.
    /// </summary>
    /// <param name="value">The result value</param>
    /// <returns>A new result</returns>
    protected static IResult<TResponse> Ok(TResponse value)
    {
        return Result.Ok(value);
    }

    /// <summary>
    /// Create a failed result for the event.
    /// </summary>
    /// <param name="failures">A set of failures</param>
    /// <returns>A new result</returns>
    protected static IResult<TResponse> Fail(params IFailure[] failures)
    {
        return Fail(failures.AsEnumerable());
    }

    /// <summary>
    /// Create a failed result for the event.
    /// </summary>
    /// <param name="failures">A set of failures</param>`
    /// <returns>A new result</returns>
    protected static IResult<TResponse> Fail(IEnumerable<IFailure> failures)
    {
        return Result.Fail<TResponse>(failures);
    }
}
