using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Action.Railway
{
    /// <summary>
    /// This class holds the internal logic for the Result abstract class and its inheritors.
    /// InternalResult should not be directly accessed outside of its Result wrapper.
    /// </summary>
    internal sealed class InternalResult
    {
        public IEnumerable<IFailure> Failures => _failures;
        public bool IsSuccess => !IsFailed;
        public bool IsFailed { get; }

        private readonly List<IFailure> _failures = new List<IFailure>();

        public InternalResult(bool isFailure, IEnumerable<IFailure> failures)
        {
            if (isFailure)
            {
                if (failures == null)
                {
                    throw new ArgumentNullException(nameof(failures), "Failures must not be null for a failed result.");
                }
                if (!failures.Any())
                {
                    throw new ArgumentException("Failures must be provided for failed result.", nameof(failures));
                }

                _failures.AddRange(failures);
            }
            else if (failures != null)
            {
                throw new ArgumentException("Cannot provide failures for success result.", nameof(failures));
            }

            IsFailed = isFailure;
        }
    }
}
