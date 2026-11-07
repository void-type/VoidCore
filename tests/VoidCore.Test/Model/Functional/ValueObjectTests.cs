using System.Diagnostics.CodeAnalysis;
using VoidCore.Model.Functional;
using Xunit;

namespace VoidCore.Test.Model.Functional;

public class ValueObjectTests
{
    [Fact]
    public void ValueObjects_of_different_types_are_not_equal()
    {
        var temp1 = new Address("a", "b");
        var dist1 = new DerivedAddress("a", "b");

        Assert.False(temp1 == dist1);
    }

    [Fact]
    public void Null_is_equal_to_null()
    {
        Assert.True((Address)null! == null);
        Assert.True(null == (Address)null!);
    }

    [Fact]
    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Intentionally exercises the null-handling of the comparison operators.")]
    public void ValueObject_is_not_equal_to_null()
    {
        var temp1 = new Address("a", "b");

        Assert.False(temp1.Equals(null));
        Assert.False(temp1 is null);
        Assert.False(null == temp1);
    }

    [Fact]
    public void ValueObject_is_not_equal_to_ValueObject_with_different_value()
    {
        var temp1 = new Address("a", "b");
        var temp2 = new Address("b", "a");

        Assert.False(temp1 == temp2);
        Assert.True(temp1 != temp2);
        Assert.NotEqual(temp1, temp2);
        Assert.NotEqual(temp1.GetHashCode(), temp2.GetHashCode());
    }

    [Fact]
    public void ValueObjects_are_equal_if_values_are_same()
    {
        var temp1 = new Address("a", "b");
        var temp2 = new Address("a", "b");

        Assert.True(temp1 == temp2);
        Assert.False(temp1 != temp2);
        Assert.Equal(temp1, temp2);
        Assert.Equal(temp1.GetHashCode(), temp2.GetHashCode());
    }

    [Fact]
    public void ValueObjects_can_be_ordered_with_comparison_operators()
    {
        var low = new Address("a", "b");
        var same = new Address("a", "b");
        var high = new Address("a", "c");

        Assert.True(low < high);
        Assert.True(low <= high);
        Assert.True(low <= same);
        Assert.True(high > low);
        Assert.True(high >= low);
        Assert.True(low >= same);
        Assert.False(high < low);
        Assert.False(low < same);
        Assert.False(low > same);
    }

    [Fact]
    public void Null_sorts_before_any_ValueObject_in_comparison_operators()
    {
        var temp1 = new Address("a", "b");
        var nullAddress = (Address)null!;
        var otherNull = (Address)null!;

        Assert.True(nullAddress < temp1);
        Assert.True(nullAddress <= temp1);
        Assert.False(nullAddress > temp1);
        Assert.False(nullAddress >= temp1);
        Assert.True(temp1 > nullAddress);
        Assert.True(temp1 >= nullAddress);
        Assert.False(temp1 < nullAddress);
        Assert.False(temp1 <= nullAddress);
        Assert.True(nullAddress >= otherNull);
        Assert.True(nullAddress <= otherNull);
    }

    [Fact]
    public void CompareTo_null_returns_positive()
    {
        var temp1 = new Address("a", "b");

        Assert.Equal(1, temp1.CompareTo((object?)null));
        Assert.Equal(1, temp1.CompareTo((ValueObject?)null));
    }

    private class Address : ValueObject
    {
        public Address(string street, string city)
        {
            Street = street;
            City = city;
        }

        public string Street { get; }
        public string City { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
        }
    }

    private class DerivedAddress : Address
    {
        public DerivedAddress(string street, string city) : base(street, city) { }
    }
}
