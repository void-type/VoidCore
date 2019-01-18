using System.Collections.Generic;

namespace VoidCore.Domain
{
    /// <summary>
    /// The result of a fallible operation that does not return a value on success.
    /// Generally used with CQRS Commands or other void fallible operations.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public sealed partial class Result : ResultAbstract
    {
        private Result() { }

        private Result(IEnumerable<IFailure> failures) : base(failures) { }
    }
}
