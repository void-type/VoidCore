using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Action.Railway
{
    public class Result<TSuccess> : IFallible where TSuccess : class
    {
        public IEnumerable<IValidationError> Failure => _failures;
        public bool IsSuccess => !IsFailed;
        public bool IsFailed => Failure.Any();
        public TSuccess Success { get; }
        public bool SuccessHasValue => Success != default(TSuccess);

        public static Result<TSuccess> Ok(TSuccess success)
        {
            return new Result<TSuccess>(success);
        }

        public static Result<TSuccess> Fail(IEnumerable<IValidationError> failures)
        {
            return new Result<TSuccess>(failures);
        }

        public static Result<TSuccess> Fail(string errorMessage, string uiHandle = null)
        {
            return new Result<TSuccess>(new ValidationError(errorMessage, uiHandle));
        }

        public static Result<TSuccess> CombineFailures(params Result<TSuccess>[] results)
        {
            var failedResults = results.Where(result => result.IsFailed).SelectMany(result => result.Failure);

            return new Result<TSuccess>(failedResults);
        }

        private Result(TSuccess success)
        {
            Success = success;
        }

        private Result(IEnumerable<IValidationError> failures)
        {
            _failures.AddRange(failures);
        }

        private Result(IValidationError failure)
        {
            _failures.Add(failure);
        }
        private readonly List<IValidationError> _failures = new List<IValidationError>();

    }
}
