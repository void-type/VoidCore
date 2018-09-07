using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Railway
{
    public abstract class DomainEventHandlerAbstract<TRequest, TResponse> : IDomainEventHandler<TRequest, TResponse>
    {
        public async Task<Result<TResponse>> Handle(TRequest request)
        {
            var validation = _validators.Select(v => v.Validate(request)).Combine();

            Result<TResponse> result;

            if (validation.IsSuccess)
            {
                result = await HandleInternal(request);
            }
            else
            {
                result = Result.Fail<TResponse>(validation.Failures);
            }

            foreach (var processor in _postProcessors)
            {
                await processor.Process(request, result);
            }

            return result;
        }

        protected DomainEventHandlerAbstract<TRequest, TResponse> AddValidator(IValidator<TRequest> validator)
        {
            _validators.Add(validator);
            return this;
        }

        protected DomainEventHandlerAbstract<TRequest, TResponse> AddPostProcessor(IPostProcessor<TRequest, TResponse> processor)
        {
            _postProcessors.Add(processor);
            return this;
        }

        protected abstract Task<Result<TResponse>> HandleInternal(TRequest request);

        private readonly List<IValidator<TRequest>> _validators = new List<IValidator<TRequest>>();
        private readonly List<IPostProcessor<TRequest, TResponse>> _postProcessors = new List<IPostProcessor<TRequest, TResponse>>();
    }
}
