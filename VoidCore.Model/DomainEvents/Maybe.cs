using System;

namespace VoidCore.Model.DomainEvents
{
    /// <summary>
    /// A class for avoiding null return values from an operation. Provides conversion and equality
    /// for the inner type.
    /// Modified from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public sealed class Maybe<T> : IEquatable<Maybe<T>>
    {
        /// <summary>
        /// Get a new empty Maybe of type T
        /// </summary>
        /// <returns>A new empty Maybe of T</returns>
        public static Maybe<T> None => new Maybe<T>();

        /// <summary>
        /// Checks if the Maybe has a value.
        /// </summary>
        public bool HasValue => _value != null;

        /// <summary>
        /// Check if the Maybe doesn't have a value.
        /// </summary>
        public bool HasNoValue => !HasValue;

        /// <summary>
        /// Get the value of Maybe.
        /// </summary>
        /// <value>The value of the Maybe</value>
        /// <exception cref="System.InvalidOperationException">Throws an InvalidOperationException if accessed and there is no value.</exception>
        public T Value
        {
            get
            {
                if (HasNoValue)
                {
                    throw new InvalidOperationException("Do not access the value of Maybe if it has no value.");
                }

                return _value.Value;
            }
        }

        /// <summary>
        /// Construct an empty Maybe
        /// </summary>
        private Maybe()
        {
            _value = null;
        }

        /// <summary>
        /// Construct a Maybe with value.
        /// </summary>
        /// <param name="value"></param>
        private Maybe(T value)
        {
            _value = value == null ? null : new MaybeValueWrapper(value);
        }

        /// <summary>
        /// Convert an object to a Maybe of obj.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A new Maybe of T of obj</returns>
        public static Maybe<T> From(T obj)
        {
            return new Maybe<T>(obj);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is T)
            {
                obj = new Maybe<T>((T) obj);
            }

            if (!(obj is Maybe<T>))
            {
                return false;
            }

            var other = (Maybe<T>) obj;
            return Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(Maybe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
            {
                return true;
            }

            if (HasNoValue || other.HasNoValue)
            {
                return false;
            }

            return _value.Value.Equals(other._value.Value);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (HasNoValue)
            {
                return 0;
            }

            return _value.Value.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (HasNoValue)
            {
                return "No value";
            }

            return Value.ToString();
        }

        /// <summary>
        /// Implicitly convert a value to a Maybe of value.
        /// </summary>
        /// <param name="value">The value</param>
        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        /// <inheritdoc/>
        public static bool operator ==(Maybe<T> maybe, T value)
        {
            if (maybe.HasNoValue)
            {
                return false;
            }

            return maybe.Value.Equals(value);
        }

        /// <inheritdoc/>
        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        /// <inheritdoc/>
        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        /// <inheritdoc/>
        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        private readonly MaybeValueWrapper _value;

        /// <summary>
        /// This class wraps the inner value to allow Maybe to work with non-nullable value types.
        /// We can set a reference to this type to null whereas we can't do that with NNVTs.
        /// </summary>
        private class MaybeValueWrapper
        {
            public MaybeValueWrapper(T value)
            {
                Value = value;
            }

            internal readonly T Value;
        }
    }
}
