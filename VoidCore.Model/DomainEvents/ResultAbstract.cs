using System.Collections.Generic;

namespace VoidCore.Model.DomainEvents
{
    /// <summary>
    /// Base class for Result classes.
    /// Inspired by https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public abstract class ResultAbstract : IResult
    {
        /// <inheritdoc/>
        public IEnumerable<IFailure> Failures => _internalResult.Failures;

        /// <inheritdoc/>
        public bool IsSuccess => _internalResult.IsSuccess;

        /// <inheritdoc/>
        public bool IsFailed => _internalResult.IsFailed;

        private readonly ResultInternal _internalResult;

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
    }
}
