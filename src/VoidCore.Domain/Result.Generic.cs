using System;
using System.Collections.Generic;

namespace VoidCore.Domain
{
    /// <summary>
    /// The result of a fallible operation that returns a value on success.
    /// Generally used with CQRS Queries or other non-void fallible operations.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    /// <typeparam name="T">The type of value to return on success</typeparam>
    public sealed class Result<T> : ResultAbstract, IResult<T>
    {
        /// <summary>
        /// The success value
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws an InvalidOperationException if accessed on a failed Result.</exception>
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

        internal Result(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value),
                    "Cannot set a result value of null. Use non-generic Result for results without values.");
            }

            _value = value;
        }

        internal Result(IEnumerable<IFailure> failures) : base(failures) { }

        private T _value;
    }
}
