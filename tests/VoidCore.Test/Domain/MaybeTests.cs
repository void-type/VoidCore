using System;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class MaybeTests
    {
        [Fact]
        public void Accessing_value_of_none_throws_InvalidOperationException()
        {
            var maybe = Maybe<string>.None;
            Assert.Throws<InvalidOperationException>(() =>
            {
                var unused = maybe.Value;
            });
        }

        [Fact]
        public void Maybe_from_null_has_no_value_and_is_none()
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
        public void Maybe_from_string_has_value()
        {
            var maybe = Maybe.From("some value");
            Assert.False(maybe.HasNoValue);
            Assert.True(maybe.HasValue);
            Assert.Equal("some value", maybe.Value);
            Assert.NotEqual(maybe, Maybe<string>.None);
        }

        [Fact]
        public void Maybe_from_int_has_value()
        {
            var maybe = Maybe.From(2);
            Assert.False(maybe.HasNoValue);
            Assert.True(maybe.HasValue);
            Assert.Equal(2, maybe.Value);
            Assert.NotEqual(maybe, Maybe<int>.None);
        }

        [Fact]
        public void Maybe_has_implicit_conversion_from_value()
        {
            Maybe<string> maybe = "some value";
            Assert.False(maybe.HasNoValue);
            Assert.True(maybe.HasValue);
            Assert.Equal("some value", maybe.Value);
            Assert.NotEqual(maybe, Maybe<string>.None);
        }

        [Fact]
        public void Maybe_GetHashCode_returns_value_hash_except_when_none()
        {
            const string value = "some value";
            var maybe = Maybe.From(value);
            Assert.Equal(value.GetHashCode(), maybe.GetHashCode());
        }

        [Fact]
        public void None_GetHashCode_returns_zero()
        {
            var maybe = Maybe<string>.None;
            Assert.Equal(0, maybe.GetHashCode());
        }

        [Fact]
        public void Maybe_is_equal_to_maybe_as_object()
        {
            var maybe1 = Maybe.From("some value");
            var maybe2 = Maybe.From("some value") as object;
            Assert.True(maybe1.Equals(maybe2));
        }

        [Fact]
        public void Maybe_is_equal_to_value_as_object()
        {
            var maybe = Maybe.From("some value");
            object other = "some value";
            Assert.True(maybe.Equals(other));
        }

        [Fact]
        public void Maybe_is_not_equal_to_another_object_with_different_value()
        {
            var maybe = Maybe.From("some value");
            object other = 3;
            Assert.False(maybe.Equals(other));
        }

        [Fact]
        public void Maybe_is_equal_to_its_value()
        {
            var maybe = Maybe.From("some value");
            Assert.True(maybe == "some value");
            Assert.False(maybe != "some value");
            Assert.True(maybe.Equals("some value"));
        }

        [Fact]
        public void Maybe_is_equal_to_maybe_with_same_value()
        {
            var maybe1 = Maybe.From("some value");
            var maybe2 = Maybe.From("some value");
            Assert.True(maybe1 == maybe2);
            Assert.False(maybe1 != maybe2);
            Assert.True(maybe1.Equals(maybe2));
        }

        [Fact]
        public void Maybe_is_not_equal_to_a_different_value()
        {
            var maybe = Maybe.From("some value");
            Assert.False(maybe == "some other value");
            Assert.True(maybe != "some other value");
            Assert.False(maybe.Equals("some other value"));
        }

        [Fact]
        public void Maybe_is_not_equal_to_maybe_with_a_different_value()
        {
            var maybe1 = Maybe.From("some value");
            var maybe2 = Maybe.From("some other value");
            Assert.False(maybe1 == maybe2);
            Assert.True(maybe1 != maybe2);
            Assert.False(maybe1.Equals(maybe2));
        }

        [Fact]
        public void Maybe_is_not_equal_to_null()
        {
            var maybe = Maybe.From("some value");
            Assert.False(maybe == null);
            Assert.False(null == maybe);
            Assert.True(maybe != null);
            Assert.True(null != maybe);
            Assert.False(maybe.Equals(null));
        }

        [Fact]
        public void Maybe_is_not_equal_to_none()
        {
            var maybe1 = Maybe.From("some value");
            var maybe2 = Maybe<string>.None;
            Assert.False(maybe1 == maybe2);
            Assert.True(maybe1 != maybe2);
            Assert.False(maybe1.Equals(maybe2));
        }

        [Fact]
        public void None_is_not_equal_to_maybe_with_value()
        {
            var maybe1 = Maybe<string>.None;
            var maybe2 = Maybe.From("some value");
            Assert.False(maybe1 == maybe2);
            Assert.True(maybe1 != maybe2);
        }

        [Fact]
        public void None_is_not_equal_to_a_value()
        {
            var maybe = Maybe<string>.None;
            Assert.False(maybe == "some other value");
            Assert.True(maybe != "some other value");
        }

        [Fact]
        public void None_is_not_equal_to_null()
        {
            var maybe = Maybe<string>.None;
            Assert.False(maybe == null);
            Assert.False(null == maybe);
            Assert.True(maybe != null);
            Assert.True(null != maybe);
            Assert.False(maybe.Equals(null));
        }

        [Fact]
        public void Null_maybe_is_equal_to_null()
        {
            Maybe<string> nullMaybe = null;

            Assert.True(nullMaybe == null);
            Assert.True(null == nullMaybe);
            Assert.True(nullMaybe is null);

            Assert.False(nullMaybe == "some value");
        }

        [Fact]
        public void Maybe_to_string_returns_value_ToString()
        {
            var maybe = Maybe.From("some value");
            Assert.Equal("some value", maybe.ToString());
        }

        [Fact]
        public void None_to_string_returns_no_value()
        {
            var maybe = Maybe<string>.None;
            Assert.Equal("No value", maybe.ToString());
        }
    }
}
