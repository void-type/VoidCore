using System.Collections.Generic;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Action.Railway
{
    public interface IFallible
    {
        IEnumerable<IValidationError> Failure { get; }
        bool IsSuccess { get; }
        bool IsFailed { get; }
    }
}
