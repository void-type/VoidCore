using System.Diagnostics.CodeAnalysis;

namespace VoidCore.Model.Functional;

/// <summary>
/// A class for avoiding null return values from an operation. Provides conversion and equality for the inner type.
/// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
/// <typeparam name="T">The value of the maybe</typeparam>
public sealed class Maybe<T> : IEquatable<Maybe<T>>
{
    private readonly InternalValueWrapper<T>? _value;

    /// <summary>
    /// Construct an empty Maybe
    /// </summary>
    internal Maybe() { }

    /// <summary>
    /// Construct a Maybe with value.
    /// </summary>
    /// <param name="value">The value to create the Maybe from</param>
    internal Maybe(T? value)
    {
        _value = value is null ? null : new InternalValueWrapper<T>(value);
    }

    /// <summary>
    /// Check if the Maybe doesn't have a value.
    /// </summary>
    public bool HasNoValue => _value is null;

    /// <summary>
    /// Checks if the Maybe has a value.
    /// </summary>
    public bool HasValue => !HasNoValue;

    /// <summary>
    /// Get the value of Maybe.
    /// </summary>
    /// <value>The value of the Maybe</value>
    /// <exception cref="InvalidOperationException">Throws when accessing value of a maybe without a value.</exception>
    [NotNull]
    public T Value
    {
        get
        {
            if (HasNoValue)
            {
                throw new InvalidOperationException("Do not access the value of Maybe if it has no value.");
            }

            return _value!.Value;
        }
    }

    /// <summary>
    /// Implicitly convert a value to a Maybe of value.
    /// </summary>
    /// <param name="value">The value</param>
    public static implicit operator Maybe<T>(T? value)
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
        if (ReferenceEquals(maybe, value))
        {
            return true;
        }

        if (maybe is null)
        {
            return false;
        }

        return !maybe.HasNoValue && maybe.Value!.Equals(value);
    }

    /// <summary>
    /// Compares equality of this and another maybe
    /// </summary>
    /// <param name="first">This maybe</param>
    /// <param name="second">Another maybe to be compared</param>
    /// <returns>A boolean result of equality</returns>
    public static bool operator ==(Maybe<T> first, Maybe<T> second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }

        return first?.Equals(second) == true;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is T value)
        {
            obj = new Maybe<T>(value);
        }

        if (obj is not Maybe<T>)
        {
            return false;
        }

        var other = (Maybe<T>)obj;
        return Equals(other);
    }

    /// <inheritdoc/>
    public bool Equals(Maybe<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (HasNoValue && other.HasNoValue)
        {
            return true;
        }

        if (HasNoValue || other.HasNoValue)
        {
            return false;
        }

        return Value.Equals(other._value!.Value);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HasNoValue ? 0 : Value.GetHashCode();
    }

    /// <inheritdoc/>
    public override string? ToString()
    {
        return HasNoValue ? "No value" : Value.ToString();
    }
}
