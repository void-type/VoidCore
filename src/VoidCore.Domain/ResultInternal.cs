using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain.Guards;

namespace VoidCore.Domain
{
    /// <summary>
    /// This class holds the internal logic for the Result abstract class and its inheritors. InternalResult should not
    /// be directly accessed outside of its Result wrapper. This class shares its constructor logic with inheritors.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    internal sealed class ResultInternal
    {
        internal ResultInternal(IEnumerable<IFailure> failures)
        {
            var failuresArray = failures as IFailure[] ?? failures?.ToArray();

            failuresArray.EnsureNotNullOrEmpty(nameof(failures));

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

        public IEnumerable<IFailure> Failures { get; } = new IFailure[0];
        public bool IsFailed { get; }
        public bool IsSuccess => !IsFailed;
    }
}
