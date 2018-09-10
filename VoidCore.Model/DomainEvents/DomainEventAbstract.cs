using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Model.Validation;

namespace VoidCore.Model.DomainEvents
{
    /// <inheritdoc/>
    public abstract class DomainEventAbstract<TRequest, TResponse> : IDomainEvent<TRequest, TResponse>
    {
        /// <inheritdoc/>
        public async Task<Result<TResponse>> Handle(TRequest request)
        {
            var validation = _validators.Select(v => v.Validate(request)).Combine();

            var result = validation.IsSuccess ?
                await HandleInternal(request) :
                Result.Fail<TResponse>(validation.Failures);

            _postProcessors.ForEach(p => p.Process(request, result));

            return result;
        }

        /// <summary>
        /// Add a validator to validate the request. All validators are run before checking results.
        /// </summary>
        /// <param name="validator">The IValidator</param>
        /// <returns>The event for chaining setup commands</returns>
        public DomainEventAbstract<TRequest, TResponse> AddRequestValidator(IValidator<TRequest> validator)
        {
            _validators.Add(validator);
            return this;
        }

        /// <summary>
        /// Add a post processor to run after the event has been handled.
        /// </summary>
        /// <param name="processor">The IPostProcessor</param>
        /// <returns>The event for chaining setup commands</returns>
        public DomainEventAbstract<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
        {
            _postProcessors.Add(processor);
            return this;
        }

        /// <summary>
        /// Override this method to provide domain logic to handle the validated request and return an appropriate result of the response.
        /// Cross-cutting concerns or non-domain elements should be put into post processors.
        /// </summary>
        /// <param name="request">The validated request</param>
        /// <returns>A result of TResponse</returns>
        protected abstract Task<Result<TResponse>> HandleInternal(TRequest request);

        private readonly List<IValidator<TRequest>> _validators = new List<IValidator<TRequest>>();
        private readonly List<IPostProcessor<TRequest, TResponse>> _postProcessors = new List<IPostProcessor<TRequest, TResponse>>();
    }
}
