using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class ResultTests
    {
        [Fact]
        public void AccessingFailedResultValueThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var unused = Result.Fail<object>("some error").Value;
            });
        }

        [Fact]
        public void CombineWithFailuresGivesFailures()
        {
            var result = new List<Result>() { Result.Ok(), Result.Fail("oops"), Result.Fail("oops"), Result.Ok() }
                .Combine();

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void CombineWithNoFailuresGivesSuccess()
        {
            var result = new List<Result>() { Result.Ok(), Result.Ok() }
                .Combine();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void ImplicitConversionFromTypedToUntypedProvidesCorrectVariant()
        {
            var source = Result.Ok("good");
            Result result = source;
            Assert.True(result.IsSuccess);

            source = Result.Fail<string>("oops");
            result = source;
            Assert.True(result.IsFailed);
        }

        [Fact]
        public void ResultIsFailedFailure()
        {
            var result = Result.Fail(new Failure("Some error", "someHandle"));
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.Single().Message);
            Assert.Equal("someHandle", result.Failures.Single().UiHandle);
        }

        [Fact]
        public void ResultIsFailedFailures()
        {
            var result = Result.Fail(new IFailure[]
            {
                new Failure("Some error", "someHandle"),
                    new Failure("Some error", "someHandle")
            });
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.First().Message);
            Assert.Equal("someHandle", result.Failures.First().UiHandle);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void ResultIsFailedStrings()
        {
            var result = Result.Fail("Some error", "someHandle");
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.Single().Message);
            Assert.Equal("someHandle", result.Failures.Single().UiHandle);
        }

        [Fact]
        public void ResultIsSuccess()
        {
            var result = Result.Ok();
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void ResultThrowsErrorWithEmptyFailures()
        {
            Assert.Throws<ArgumentException>(() => Result.Fail(new IFailure[0]));
            Assert.Throws<ArgumentException>(() => Result.Fail(new List<IFailure>()));
        }

        [Fact]
        public void ResultThrowsErrorWithNullFailures()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Fail((IFailure[])null));
            Assert.Throws<ArgumentNullException>(() => Result.Fail((IFailure)null));
        }

        [Fact]
        public void TypedCombineWithFailuresGivesFailures()
        {
            var result = new List<Result<string>>() { Result.Ok(""), Result.Fail<string>(""), Result.Fail<string>(""), Result.Ok("") }
                .Combine();

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void TypedCombineWithNoFailuresGivesSuccess()
        {
            var result = new List<Result<string>>() { Result.Ok(""), Result.Ok("") }
                .Combine();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void TypedResultIsFailedFailure()
        {
            var result = Result.Fail<string>(new Failure("Some error", "someHandle"));
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.Single().Message);
            Assert.Equal("someHandle", result.Failures.Single().UiHandle);
        }

        [Fact]
        public void TypedResultIsFailedFailures()
        {
            var result = Result.Fail<string>(new IFailure[] { new Failure("Some error", "someHandle"), new Failure("Some error", "someHandle") });
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.First().Message);
            Assert.Equal("someHandle", result.Failures.First().UiHandle);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void TypedResultIsFailedStrings()
        {
            var result = Result.Fail<string>("Some error", "someHandle");
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.Single().Message);
            Assert.Equal("someHandle", result.Failures.Single().UiHandle);
        }

        [Fact]
        public void TypedResultIsSuccess()
        {
            var result = Result.Ok("success");
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
            Assert.Equal("success", result.Value);
        }

        [Fact]
        public void TypedResultThrowsErrorWithEmptyFailures()
        {
            Assert.Throws<ArgumentException>(() => Result.Fail<string>(new IFailure[0]));
        }

        [Fact]
        public void TypedResultThrowsErrorWithEmptySuccess()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Ok((string)null));
        }

        [Fact]
        public void TypedResultThrowsErrorWithNullFailures()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Fail<string>((IFailure[])null));
            Assert.Throws<ArgumentNullException>(() => Result.Fail<string>((IFailure)null));
        }
    }
}