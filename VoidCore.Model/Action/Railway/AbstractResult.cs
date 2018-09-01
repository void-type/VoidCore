using System.Collections.Generic;

namespace VoidCore.Model.Action.Railway
{
    /// <summary>
    /// Base class for Result classes.
    /// </summary>
    public abstract class AbstractResult : IFallible
    {
        /// <inheritdoc />
        public IEnumerable<IFailure> Failures => _internalResult.Failures;

        /// <inheritdoc />
        public bool IsSuccess => _internalResult.IsSuccess;

        /// <inheritdoc />
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
