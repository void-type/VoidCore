using System;
using System.Collections.Generic;
using VoidCore.Model.Domain;
using Xunit;

namespace VoidCore.Test.Model.Domain
{
    public class ValueObjectTests
    {
        [Fact]
        public void CannotCompareVosOfDifferentTypes()
        {
            var temp1 = new Temperature(20.1, Temperature.UnitType.C);
            var dist1 = new Distance(20.1, Distance.UnitType.Km);

            Assert.Throws<ArgumentException>(() => temp1 == dist1);
        }

        [Fact]
        public void VoComparedToNullAreNotEqual()
        {
            var temp1 = new Temperature(20.1, Temperature.UnitType.C);

            Assert.False(temp1.Equals(null));
            Assert.False(temp1 == null);
            Assert.False(null == temp1);
        }

        [Fact]
        public void VosWithDifferentValuesAreNotEqual()
        {
            var temp1 = new Temperature(20.1, Temperature.UnitType.C);
            var temp2 = new Temperature(20.1, Temperature.UnitType.F);

            Assert.False(temp1 == temp2);
            Assert.True(temp1 != temp2);
            Assert.NotEqual(temp1, temp2);
            Assert.NotEqual(temp1.GetHashCode(), temp2.GetHashCode());
        }

        [Fact]
        public void VosWithSameValuesAreEqual()
        {
            var temp1 = new Temperature(20.1, Temperature.UnitType.C);
            var temp2 = new Temperature(20.1, Temperature.UnitType.C);

            Assert.True(null == (Temperature) null);

            Assert.True(temp1 == temp2);
            Assert.False(temp1 != temp2);
            Assert.Equal(temp1, temp2);
            Assert.Equal(temp1.GetHashCode(), temp2.GetHashCode());
        }

        internal class Distance : ValueObject
        {
            public enum UnitType
            {
                Mi,
                Km
            }

            public double Reading { get; }
            public UnitType Unit { get; }

            public Distance(double reading, UnitType unit)
            {
                Reading = reading;
                Unit = unit;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Reading;
                yield return Unit;
            }
        }

        internal class Temperature : ValueObject
        {
            public enum UnitType
            {
                F,
                C
            }

            public double Reading { get; }
            public UnitType Unit { get; }

            public Temperature(double reading, UnitType unit)
            {
                Reading = reading;
                Unit = unit;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Reading;
                yield return Unit;
            }
        }
    }
}
