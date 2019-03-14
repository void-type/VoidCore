using System;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class MaybeTests
    {
        [Fact]
        public void MaybeFromNullHasNoValueAndIsNoneMaybe()
        {
            var maybeFromNull = Maybe.From<string>(null);

            Assert.True(maybeFromNull.HasNoValue);
            Assert.False(maybeFromNull.HasValue);

            Assert.True(Maybe<string>.None.HasNoValue);
            Assert.False(Maybe<string>.None.HasValue);

            Assert.True(maybeFromNull == Maybe<string>.None);
            Assert.True(Maybe<string>.None == maybeFromNull);

            Assert.False(maybeFromNull != Maybe<string>.None);
            Assert.False(Maybe<string>.None != maybeFromNull);

            Assert.True(maybeFromNull.Equals(Maybe<string>.None));
            Assert.True(Maybe<string>.None.Equals(maybeFromNull));
        }

        [Fact]
        public void AccessingNoneMaybeValueThrowsInvalidOperationException()
        {
            var maybe = Maybe<string>.None;
            Assert.Throws<InvalidOperationException>(() =>
            {
                var unused = maybe.Value;
            });
        }

        [Fact]
        public void NoneMaybeIsNotEqualToMaybeWithValue()
        {
            var maybe1 = Maybe<string>.None;
            var maybe2 = Maybe.From("some value");
            Assert.False(maybe2 == maybe1);
            Assert.True(maybe2 != maybe1);
        }

        [Fact]
        public void NoneMaybeIsNotEqualToAValue()
        {
            var maybe = Maybe<string>.None;
            Assert.False(maybe == "some other value");
            Assert.True(maybe != "some other value");
        }

        [Fact]
        public void NoneMaybeIsNotEqualToNull()
        {
            var maybe = Maybe<string>.None;
            Assert.False(maybe == null);
            Assert.False(null == maybe);
            Assert.True(maybe != null);
            Assert.True(null != maybe);
            Assert.False(maybe.Equals(null));
        }

        [Fact]
        public void MaybeFromStringHasValue()
        {
            var maybe = Maybe.From("some value");
            Assert.False(maybe.HasNoValue);
            Assert.True(maybe.HasValue);
            Assert.Equal("some value", maybe.Value);
            Assert.NotEqual(maybe, Maybe<string>.None);
        }

        [Fact]
        public void MaybeFromIntHasValue()
        {
            var maybe = Maybe.From(2);
            Assert.False(maybe.HasNoValue);
            Assert.True(maybe.HasValue);
            Assert.Equal(2, maybe.Value);
            Assert.NotEqual(maybe, Maybe<int>.None);
        }

        [Fact]
        public void MaybeHasImplicitConversionFromValue()
        {
            Maybe<string> maybe = "some value";
            Assert.False(maybe.HasNoValue);
            Assert.True(maybe.HasValue);
            Assert.Equal("some value", maybe.Value);
            Assert.NotEqual(maybe, Maybe<string>.None);
        }

        [Fact]
        public void MaybeGetHashCodeReturnsValueHashCodeExceptWhenNone()
        {
            const string value = "some value";
            var maybe = Maybe.From(value);
            Assert.Equal(value.GetHashCode(), maybe.GetHashCode());

            maybe = Maybe<string>.None;
            Assert.Equal(0, maybe.GetHashCode());
        }

        [Fact]
        public void MaybeIsEqualToMaybeAsObject()
        {
            var maybe1 = Maybe.From("some value");
            var maybe2 = Maybe.From("some value") as object;
            Assert.True(maybe1.Equals(maybe2));
        }

        [Fact]
        public void MaybeIsEqualToValueAsObject()
        {
            var maybe = Maybe.From("some value");
            object other = "some value";
            Assert.True(maybe.Equals(other));
        }

        [Fact]
        public void MaybeIsNotEqualToAnotherObjectWithDifferentValue()
        {
            var maybe = Maybe.From("some value");
            object other = 3;
            Assert.False(maybe.Equals(other));
        }

        [Fact]
        public void MaybeIsEqualWithItsValue()
        {
            var maybe = Maybe.From("some value");
            Assert.True(maybe == "some value");
            Assert.False(maybe != "some value");
            Assert.True(maybe.Equals("some value"));
        }

        [Fact]
        public void MaybeIsEqualWithSameValuedMaybe()
        {
            var maybe1 = Maybe.From("some value");
            var maybe2 = Maybe.From("some value");
            Assert.True(maybe1 == maybe2);
            Assert.False(maybe1 != maybe2);
            Assert.True(maybe1.Equals(maybe2));
        }

        [Fact]
        public void MaybeIsNotEqualWithADifferentValue()
        {
            var maybe = Maybe.From("some value");
            Assert.False(maybe == "some other value");
            Assert.True(maybe != "some other value");
            Assert.False(maybe.Equals("some other value"));
        }

        [Fact]
        public void MaybeIsNotEqualWithDifferentValuedMaybe()
        {
            var maybe1 = Maybe.From("some value");
            var maybe2 = Maybe.From("some other value");
            Assert.False(maybe1 == maybe2);
            Assert.True(maybe1 != maybe2);
            Assert.False(maybe1.Equals(maybe2));
        }

        [Fact]
        public void MaybeIsNotEqualToNull()
        {
            var maybe = Maybe.From("some value");
            Assert.False(maybe == null);
            Assert.False(null == maybe);
            Assert.True(maybe != null);
            Assert.True(null != maybe);
            Assert.False(maybe.Equals(null));
        }

        [Fact]
        public void MaybeToString()
        {
            var maybe = Maybe.From("some value");
            Assert.Equal("some value", maybe.ToString());

            maybe = Maybe<string>.None;
            Assert.Equal("No value", maybe.ToString());
        }

        [Fact]
        public void NullMaybeIsEqualToNull()
        {
            Maybe<string> nullMaybe = null;

            Assert.True(nullMaybe == null);
            Assert.True(null == nullMaybe);
            Assert.True(nullMaybe is null);

            Assert.False(nullMaybe == "some value");
        }
    }
}
