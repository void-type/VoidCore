using System;
using System.Collections.Generic;
using VoidCore.Domain.Guards;

namespace VoidCore.Domain
{
    /// <summary>
    /// The result of a fallible operation that returns a value on success. Generally used with CQRS Queries or other
    /// non-void fallible operations. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    /// <typeparam name="T">The type of value to return on success</typeparam>
    public sealed class Result<T> : ResultAbstract, IResult<T>
    {
        private readonly T _value;

        internal Result(T value)
        {
            _value = value.EnsureNotNull(nameof(value), "Cannot set a result value of null. Use non-generic Result for results without values.");
        }

        internal Result(IEnumerable<IFailure> failures) : base(failures) { }

        /// <summary>
        /// The success value
        /// </summary>
        public T Value
        {
            get
            {
                if (IsFailed)
                {
                    throw new InvalidOperationException("Do not access the value of Result if it is failed.");
                }

                return _value;
            }
        }
    }
}
