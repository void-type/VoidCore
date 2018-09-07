using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Railway;
using VoidCore.Model.Validation;

namespace VoidCore.MediatR
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : Result
    {
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var result = _validators
                .Select(validator => validator.Validate(request))
                .Combine();

            if (result.IsFailed)
            {
                return result as TResponse;
            }

            return await next();
        }

        private readonly IEnumerable<IValidator<TRequest>> _validators;
    }
}
