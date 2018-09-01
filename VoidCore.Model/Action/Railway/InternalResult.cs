using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Action.Railway
{
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
            }
            else if (failures.Any())
            {
                throw new ArgumentException("Cannot provide failures for .", nameof(failures));
            }

            IsFailed = isFailure;
            _failures.AddRange(failures);
        }
    }
}
