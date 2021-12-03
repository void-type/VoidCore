using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Functional;

namespace VoidCore.Model.Events;

/// <summary>
/// An event in the domain that synchronously takes a request and returns a response.
/// </summary>
/// <typeparam name="TRequest">The type of the event request</typeparam>
/// <typeparam name="TResponse">The type of the event response</typeparam>
public abstract class EventHandlerSyncAbstract<TRequest, TResponse> : EventHandlerAbstract<TRequest, TResponse>
{
    /// <inheritdoc/>
    public override Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HandleSync(request));
    }

    /// <summary>
    /// Override this method to provide domain logic to handle the validated request and return an appropriate result
    /// of the response.
    /// </summary>
    /// <param name="request">The validated request</param>
    /// <returns>A result of TResponse</returns>
    protected abstract IResult<TResponse> HandleSync(TRequest request);
}
