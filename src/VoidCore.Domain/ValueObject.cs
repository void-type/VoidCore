using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Domain
{
    /// <summary>
    /// Prevent primitive obsession by creating named types for any value. Provides equality comparison via a list of
    /// comparable components. These work best with immutable properties. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Compares equality of two ValueObjects
        /// </summary>
        /// <param name="first">This valueObject</param>
        /// <param name="second">The valueObject to compare to</param>
        /// <returns>The boolean result of equality</returns>
        public static bool operator ==(ValueObject first, ValueObject second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }

        /// <summary>
        /// Compares inequality of two ValueObjects
        /// </summary>
        /// <param name="first">This valueObject</param>
        /// <param name="second">The valueObject to compare to</param>
        /// <returns>The boolean result of inequality</returns>
        public static bool operator !=(ValueObject first, ValueObject second)
        {
            return !(first == second);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var valueObject = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        /// <summary>
        /// Override this method to provide a list of components to compare equality with. These components can be raw or
        /// transformed properties.
        /// </summary>
        /// <returns>A list of components used to check equality of the value object</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();
    }
}
