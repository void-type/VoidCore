using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.DomainEvents;
using Xunit;

namespace VoidCore.Test.Model.DomainEvents
{
    public class ResultTests
    {
        [Fact]
        public void ResultIsSuccess()
        {
            var result = Result.Ok();
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
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
        public void TypedResultIsFailedStrings()
        {
            var result = Result.Fail<string>("Some error", "someHandle");
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.Single().Message);
            Assert.Equal("someHandle", result.Failures.Single().UiHandle);
            Assert.Null(result.Value);
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
            Assert.Null(result.Value);
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
            Assert.Null(result.Value);
        }

        [Fact]
        public void ResultThrowsErrorWithEmptyFailures()
        {
            Assert.Throws<ArgumentException>(() => Result.Fail(new IFailure[0]));
        }

        [Fact]
        public void ResultThrowsErrorWithNullFailures()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Fail((IFailure[]) null));
            Assert.Throws<ArgumentNullException>(() => Result.Fail((IFailure) null));
        }

        [Fact]
        public void TypedResultThrowsErrorWithEmptySuccess()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Ok((string) null));
        }

        [Fact]
        public void TypedResultThrowsErrorWithEmptyFailures()
        {
            Assert.Throws<ArgumentException>(() => Result.Fail<string>(new IFailure[0]));
        }

        [Fact]
        public void TypedResultThrowsErrorWithNullFailures()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Fail<string>((IFailure[]) null));
            Assert.Throws<ArgumentNullException>(() => Result.Fail<string>((IFailure) null));
        }

        [Fact]
        public void CombineWithNoFailuresGivesSuccess()
        {
            var results = new List<Result>() { Result.Ok(), Result.Ok() }.ToArray();
            var result = Result.Combine(results);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void TypedCombineWithNoFailuresGivesSuccess()
        {
            var results = new List<Result<string>>() { Result.Ok(""), Result.Ok("") }.ToArray();
            var result = Result.Combine(results);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void CombineWithFailuresGivesFailures()
        {
            var results = new List<Result>() { Result.Ok(), Result.Fail("oops"), Result.Fail("oops"), Result.Ok() }.ToArray();
            var result = Result.Combine(results);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void TypedCombineWithFailuresGivesFailures()
        {
            var results = new List<Result<string>>() { Result.Ok(""), Result.Fail<string>(""), Result.Fail<string>(""), Result.Ok("") }.ToArray();
            var result = Result.Combine(results);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }
    }
}
