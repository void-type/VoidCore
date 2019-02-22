using System.Linq;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class MaybeExtensionsTests
    {
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
        public void MaybeSelectWithoutValueReturnsNone()
        {
            Maybe<string> maybe = Maybe<string>.None;
            var value = maybe.Select(v => 2);
            Assert.IsType<Maybe<int>>(value);
            Assert.True(value.HasNoValue);
        }

        [Fact]
        public void MaybeSelectWithValueReturnsTransformation()
        {
            Maybe<string> maybe = "some value";
            var value = maybe.Select(v => 2);
            Assert.IsType<Maybe<int>>(value);
            Assert.Equal(2, value.Value);
        }

        [Fact]
        public void MaybeSelectToMaybeWithoutValueReturnNone()
        {
            Maybe<string> maybe = Maybe<string>.None;
            var value = maybe.Select(v => Maybe.From(2));
            Assert.IsType<Maybe<int>>(value);
            Assert.True(value.HasNoValue);
        }

        [Fact]
        public void MaybeSelectToMaybeWithValue()
        {
            Maybe<string> maybe = "some value";
            var value = maybe.Select(v => Maybe.From(2));
            Assert.IsType<Maybe<int>>(value);
            Assert.Equal(2, value.Value);
        }

        [Fact]
        public void MaybeUnwrapWithoutValue()
        {
            Maybe<string> maybe = Maybe<string>.None;

            var valueNoDefault = maybe.Unwrap();
            Assert.Null(valueNoDefault);

            var valueWithDefault = maybe.Unwrap("default value");
            Assert.Equal("default value", valueWithDefault);

            var valueWithDefaultFactory = maybe.Unwrap(() => "default value");
            Assert.Equal("default value", valueWithDefaultFactory);
        }

        [Fact]
        public void MaybeUnwrapWithValue()
        {
            Maybe<string> maybe = "some value";

            var valueNoDefault = maybe.Unwrap();
            Assert.Equal("some value", valueNoDefault);

            var valueWithDefault = maybe.Unwrap("default value");
            Assert.Equal("some value", valueWithDefault);

            var valueWithDefaultFactory = maybe.Unwrap(() => "default value");
            Assert.Equal("some value", valueWithDefaultFactory);
        }

        [Fact]
        public void WhereWithValueAndFalsePredicateReturnsNone()
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
