using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Logging;
using VoidCore.Model.Railway;

namespace VoidCore.MediatR
{
    public class FallibleLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : IFallible
    {
        public FallibleLoggingBehavior(ILoggingService logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var result = await next();

            if (result.IsFailed)
            {
                _logger.Warn("Failures: " + string.Join(" ", result.Failures.Select(x => x.Message)));
            }

            return result;
        }

        private readonly ILoggingService _logger;
    }
}
