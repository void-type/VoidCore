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

        internal ResultAbstract(bool isFailed, IEnumerable<IFailure> failures)
        {
            _internalResult = new ResultInternal(isFailed, failures);
        }
    }
}
