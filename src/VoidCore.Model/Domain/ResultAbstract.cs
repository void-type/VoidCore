using System.Collections.Generic;

namespace VoidCore.Model.Domain
{
    /// <summary>
    /// Base class for Result classes.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public abstract class ResultAbstract : IResult
    {
        /// <inheritdoc/>
        public IEnumerable<IFailure> Failures => _internalResult.Failures;

        /// <inheritdoc/>
        public bool IsFailed => _internalResult.IsFailed;

        /// <inheritdoc/>
        public bool IsSuccess => _internalResult.IsSuccess;

        /// <summary>
        /// Construct a failed result
        /// </summary>
        internal ResultAbstract(IEnumerable<IFailure> failures)
        {
            _internalResult = new ResultInternal(failures);
        }

        /// <summary>
        /// Construct a successful result
        /// </summary>
        internal ResultAbstract()
        {
            _internalResult = new ResultInternal();
        }

        private readonly ResultInternal _internalResult;
    }
}
