using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Functional;

namespace VoidCore.Model.Events;

/// <summary>
/// An event in the domain that asynchronously takes a request and returns a response.
/// Events can be fallible, returning a Result of response.
/// </summary>
/// <typeparam name="TRequest">The type of the event request</typeparam>
/// <typeparam name="TResponse">The type of the event response</typeparam>
public interface IEventHandler<TRequest, TResponse>
{
    /// <summary>
    /// Handle the domain event. This includes validating the request, handling the event, and running any post processors.
    /// </summary>
    /// <param name="request">The request contains all the parameters to handle the event.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    /// <returns>A result of the response.</returns>
    Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default);
}
