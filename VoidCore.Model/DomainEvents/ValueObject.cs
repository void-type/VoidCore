using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.DomainEvents
{
    /// <summary>
    /// Prevent primitive obsession by creating named types for any value.
    /// Provides equality comparison via a list of comparable components.
    /// These work best with immutable properties.
    /// Modified from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Override this method to provide a list of components to compare equality with.
        /// These components can be raw or transformed properties.
        /// </summary>
        /// <returns>A list of components used to check equality of the value object</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                throw new ArgumentException($"Invalid comparison of Value Objects of different types: {GetType()} and {obj.GetType()}");
            }

            var valueObject = (ValueObject) obj;

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

        /// <inheritdoc/>
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <inheritdoc/>
        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }
}
