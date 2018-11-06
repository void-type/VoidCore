using System;

namespace VoidCore.Model.Domain
{
    /// <summary>
    /// A class for avoiding null return values from an operation. Provides conversion and equality for the inner type.
    /// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    /// <typeparam name="T">The value of the maybe</typeparam>
    public sealed class Maybe<T> : IEquatable<Maybe<T>>
    {
        /// <summary>
        /// Get a new empty Maybe of type T
        /// </summary>
        /// <returns>A new empty Maybe of T</returns>
        public static Maybe<T> None => new Maybe<T>();

        /// <summary>
        /// Check if the Maybe doesn't have a value.
        /// </summary>
        public bool HasNoValue => !HasValue;

        /// <summary>
        /// Checks if the Maybe has a value.
        /// </summary>
        public bool HasValue => _value != null;

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
        /// Convert an object to a Maybe of obj.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A new Maybe of T of obj</returns>
        public static Maybe<T> From(T obj)
        {
            return new Maybe<T>(obj);
        }

        /// <summary>
        /// Implicitly convert a value to a Maybe of value.
        /// </summary>
        /// <param name="value">The value</param>
        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        /// <summary>
        /// Compares inequality of this maybe and a value
        /// </summary>
        /// <param name="maybe">This maybe</param>
        /// <param name="value">The value to be compared</param>
        /// <returns>A boolean result of inequality</returns>
        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        /// <summary>
        /// Compares inequality of this and another maybe
        /// </summary>
        /// <param name="first">This maybe</param>
        /// <param name="second">Another maybe to be compared</param>
        /// <returns>A boolean result of inequality</returns>
        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Compares equality of this maybe and a value
        /// </summary>
        /// <param name="maybe">This maybe</param>
        /// <param name="value">The value to be compared</param>
        /// <returns>A boolean result of equality</returns>
        public static bool operator ==(Maybe<T> maybe, T value)
        {
            return !maybe.HasNoValue && maybe.Value.Equals(value);
        }

        /// <summary>
        /// Compares equality of this and another maybe
        /// </summary>
        /// <param name="first">This maybe</param>
        /// <param name="second">Another maybe to be compared</param>
        /// <returns>A boolean result of equality</returns>
        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
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
            return HasNoValue ? 0 : _value.Value.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return HasNoValue ? "No value" : Value.ToString();
        }

        private readonly MaybeValueWrapper _value;

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
        /// <param name="value">The value to create the Maybe from</param>
        private Maybe(T value)
        {
            _value = value == null ? null : new MaybeValueWrapper(value);
        }

        /// <summary>
        /// This class wraps the inner value to allow Maybe to work with non-nullable value types. We can set a reference to this type to null whereas
        /// we can't do that with NNVTs.
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

    /// <summary>
    /// Provides non-generic helpers of the generic Maybe class.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Convert an object to a Maybe of obj.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A new Maybe of the type of the object</returns>
        public static Maybe<T> From<T>(T obj)
        {
            return Maybe<T>.From(obj);
        }
    }
}
