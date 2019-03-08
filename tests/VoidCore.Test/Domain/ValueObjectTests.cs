using System.Collections.Generic;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class ValueObjectTests
    {
        [Fact]
        public void CompareValueObjectsOfDifferentTypesReturnsFalse()
        {
            var temp1 = new Address("a", "b");
            var dist1 = new DerivedAddress("a", "b");

            Assert.False(temp1 == dist1);
        }

        [Fact]
        public void NullComparedToNullValueObjectIsEqual()
        {
            Assert.True((Address) null == null);
            Assert.True(null == (Address) null);
        }

        [Fact]
        public void ValueObjectsComparedToNullAreNotEqual()
        {
            var temp1 = new Address("a", "b");

            Assert.False(temp1.Equals(null));
            Assert.False(temp1 == null);
            Assert.False(null == temp1);
        }

        [Fact]
        public void ValueObjectsWithDifferentValuesAreNotEqual()
        {
            var temp1 = new Address("a", "b");
            var temp2 = new Address("b", "a");

            Assert.False(temp1 == temp2);
            Assert.True(temp1 != temp2);
            Assert.NotEqual(temp1, temp2);
            Assert.NotEqual(temp1.GetHashCode(), temp2.GetHashCode());
        }

        [Fact]
        public void ValueObjectsWithSameValuesAreEqual()
        {
            var temp1 = new Address("a", "b");
            var temp2 = new Address("a", "b");

            Assert.True(temp1 == temp2);
            Assert.False(temp1 != temp2);
            Assert.Equal(temp1, temp2);
            Assert.Equal(temp1.GetHashCode(), temp2.GetHashCode());
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
}
