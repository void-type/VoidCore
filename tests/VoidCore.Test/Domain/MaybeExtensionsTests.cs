using System.Linq;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class MaybeExtensionsTests
    {
        [Fact]
        public void MaybeSelectToMaybeWithoutValue()
        {
            Maybe<string> maybe = Maybe<string>.None;
            var value = maybe.Select(v => Maybe<int>.From(2));
            Assert.IsType<Maybe<int>>(value);
            Assert.True(value.HasNoValue);
        }

        [Fact]
        public void MaybeSelectToMaybeWithValue()
        {
            Maybe<string> maybe = "some value";
            var value = maybe.Select(v => Maybe<int>.From(2));
            Assert.IsType<Maybe<int>>(value);
            Assert.Equal(2, value.Value);
        }

        [Fact]
        public void MaybeSelectWithoutValue()
        {
            Maybe<string> maybe = Maybe<string>.None;
            var value = maybe.Select(v => 2);
            Assert.IsType<Maybe<int>>(value);
            Assert.True(value.HasNoValue);
        }

        [Fact]
        public void MaybeSelectWithValue()
        {
            Maybe<string> maybe = "some value";
            var value = maybe.Select(v => 2);
            Assert.IsType<Maybe<int>>(value);
            Assert.Equal(2, value.Value);
        }

        [Fact]
        public void MaybeToResultWithoutValueIsFailed()
        {
            var maybe = Maybe<string>.None;
            var result = maybe.ToResult("no value", "uiField");
            Assert.False(result.IsSuccess);
            var failure = result.Failures.Single();
            Assert.Equal("no value", failure.Message);
            Assert.Equal("uiField", failure.UiHandle);
        }

        [Fact]
        public void MaybeToResultWithValueIsSuccessful()
        {
            Maybe<string> maybe = "some value";
            Assert.True(maybe.ToResult("no value").IsSuccess);
        }

        [Fact]
        public void MaybeUnwrapWithoutValue()
        {
            Maybe<string> maybe = Maybe<string>.None;
            string value = maybe.Unwrap();
            Assert.Null(value);

            value = maybe.Unwrap("default value");
            Assert.Equal("default value", value);
        }

        [Fact]
        public void MaybeUnwrapWithoutValueWithSelector()
        {
            Maybe<string> maybe = Maybe<string>.None;
            string value = maybe.Unwrap(v => v + " added");
            Assert.Null(value);
            value = maybe.Unwrap(v => v + " added", "default value");
            Assert.Equal("default value", value);
        }

        [Fact]
        public void MaybeUnwrapWithValue()
        {
            Maybe<string> maybe = "some value";
            string value = maybe.Unwrap();
            Assert.Equal("some value", value);
        }

        [Fact]
        public void MaybeUnwrapWithValueWithSelector()
        {
            Maybe<string> maybe = "some value";
            string value = maybe.Unwrap(v => v + " added");
            Assert.Equal("some value added", value);
        }

        [Fact]
        public void WhereWithFalsePredicateReturnsNone()
        {
            Maybe<string> maybe = "some value";
            var queried = maybe.Where(v => false);
            Assert.True(queried.HasNoValue);
        }

        [Fact]
        public void WhereWithoutValueAndTruePredicateReturnsNone()
        {
            var maybe = Maybe<string>.None;
            var queried = maybe.Where(v => true);
            Assert.True(queried.HasNoValue);
        }

        [Fact]
        public void WhereWithValueAndTruePredicateReturnsMaybe()
        {
            Maybe<string> maybe = "some value";
            var queried = maybe.Where(v => true);
            Assert.True(queried.HasValue);
            Assert.Equal("some value", queried.Value);
        }
    }
}
