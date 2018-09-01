using System.Collections.Generic;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Action.Railway
{
    public abstract class AbstractResult : IFallible
    {
        public IEnumerable<IFailure> Failures => _internalResult.Failures;

        public bool IsSuccess => _internalResult.IsSuccess;

        public bool IsFailed => _internalResult.IsFailed;

        private InternalResult _internalResult;

        internal AbstractResult(bool isFailed, IEnumerable<IFailure> failures)
        {
            _internalResult = new InternalResult(isFailed, failures);
        }

        internal AbstractResult(IEnumerable<IFailure> failures)
        {
            _internalResult = new InternalResult(true, failures);
        }
    }
}
