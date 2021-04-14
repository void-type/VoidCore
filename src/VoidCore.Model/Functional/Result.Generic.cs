using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Functional
{
    /// <summary>
    /// The result of a fallible operation that returns a value on success. Generally used with CQRS Queries or other
    /// non-void fallible operations. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    /// <typeparam name="T">The type of value to return on success</typeparam>
    public sealed class Result<T> : ResultAbstract, IResult<T>
    {
        private readonly InternalValueWrapper<T>? _value;

        internal Result(T value)
        {
            value.EnsureNotNull(nameof(value), "Cannot set a result value of null. Use non-generic Result for results without values.");
            _value = new InternalValueWrapper<T>(value);
        }

        internal Result(IEnumerable<IFailure> failures) : base(failures) { }

        /// <summary>
        /// The success value
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws when accessing the value of a failed result.</exception>
        [NotNull]
        public T Value
        {
            get
            {
                if (IsFailed)
                {
                    throw new InvalidOperationException("Do not access the value of Result if it is failed.");
                }

                return _value!.Value;
            }
        }
    }
}
