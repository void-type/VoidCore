using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Domain
{
    /// <summary>
    /// This class holds the internal logic for the Result abstract class and its inheritors.
    /// InternalResult should not be directly accessed outside of its Result wrapper. This class shares its constructor logic with inheritors.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    internal sealed class ResultInternal
    {
        public IEnumerable<IFailure> Failures { get; } = new IFailure[0];
        public bool IsFailed { get; }
        public bool IsSuccess => !IsFailed;

        /// <summary>
        /// Construct a failed result
        /// </summary>
        internal ResultInternal(IEnumerable<IFailure> failures)
        {
            if (failures == null)
            {
                throw new ArgumentNullException(nameof(failures), "Failures must not be null for a failed result.");
            }

            var failuresArray = failures as IFailure[] ?? failures.ToArray();
            if (!failuresArray.Any())
            {
                throw new ArgumentException("Failures must not be empty for failed result.", nameof(failures));
            }

            Failures = failuresArray;
            IsFailed = true;
        }

        /// <summary>
        /// Construct a successful result
        /// </summary>
        internal ResultInternal()
        {
            IsFailed = false;
        }
    }
}
