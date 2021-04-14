using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Events
{
    /// <summary>
    /// Construct the pipeline from the event components and store it to clean up controllers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the event request</typeparam>
    /// <typeparam name="TResponse">The type of the event response</typeparam>
    public abstract class EventPipelineAbstract<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Construct and store the InnerHandler as a complete event pipeline.
        /// </summary>
        protected IEventHandler<TRequest, TResponse>? InnerHandler;

        /// <inheritdoc/>
        public Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            return InnerHandler
                .EnsureNotNull(nameof(InnerHandler))
                .Handle(request, cancellationToken);
        }
    }
}
