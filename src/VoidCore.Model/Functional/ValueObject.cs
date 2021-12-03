using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace VoidCore.Model.Functional;

/// <summary>
/// Prevent primitive obsession by creating named types for any value. Provides equality comparison via a list of
/// comparable components. All implementations of this class should have immutable equality components.
/// These work best with immutable properties. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public abstract class ValueObject : IComparable, IComparable<ValueObject>
{
    /// <summary>
    /// Override this method to provide a list of components to compare equality with. These components can be raw or
    /// transformed properties.
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

        if (GetUnproxiedType(this) != GetUnproxiedType(obj))
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
                    return (current * 23) + (obj?.GetHashCode() ?? 0);
                }
            });
    }

    private int CompareComponents(object object1, object object2)
    {
        if (object1 is null && object2 is null)
        {
            return 0;
        }

        if (object1 is null)
        {
            return -1;
        }

        if (object2 is null)
        {
            return 1;
        }

        if (object1 is not IComparable component1 || object2 is not IComparable component2)
        {
            throw new InvalidOperationException($"Not all components in {GetUnproxiedType(this)} implement IComparable");
        }

        return component1.CompareTo(component2);
    }

    /// <inheritdoc/>
    public int CompareTo(ValueObject other)
    {
        return CompareTo(other as object);
    }

    /// <inheritdoc/>
    public int CompareTo(object obj)
    {
        var thisType = GetUnproxiedType(this);
        var otherType = GetUnproxiedType(obj);

        if (thisType != otherType)
        {
            return string.CompareOrdinal(thisType.ToString(), otherType.ToString());
        }

        var other = (ValueObject)obj;

        var components = GetEqualityComponents().ToArray();
        var otherComponents = other.GetEqualityComponents().ToArray();

        for (var i = 0; i < components.Length; i++)
        {
            var comparison = CompareComponents(components[i], otherComponents[i]);
            if (comparison != 0)
            {
                return comparison;
            }
        }

        return 0;
    }

    /// <summary>
    /// Compares equality of two ValueObjects
    /// </summary>
    /// <param name="a">This valueObject</param>
    /// <param name="b">The valueObject to compare to</param>
    /// <returns>The boolean result of equality</returns>
    public static bool operator ==(ValueObject a, ValueObject b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    /// <summary>
    /// Compares inequality of two ValueObjects
    /// </summary>
    /// <param name="a">This valueObject</param>
    /// <param name="b">The valueObject to compare to</param>
    /// <returns>The boolean result of inequality</returns>
    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    internal static Type GetUnproxiedType(object obj)
    {
        const string efCoreProxyPrefix = "Castle.Proxies.";
        const string nHibernateProxyPostfix = "Proxy";

        var type = obj.GetType();
        var typeString = type.ToString();

        if (typeString.Contains(efCoreProxyPrefix) || typeString.EndsWith(nHibernateProxyPostfix))
        {
            return type.BaseType;
        }

        return type;
    }
}
