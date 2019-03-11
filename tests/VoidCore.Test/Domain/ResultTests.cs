using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class ResultTests
    {
        [Fact]
        public void ResultThrowsErrorWithNullFailures()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Fail((List<IFailure>) null));
            Assert.Throws<ArgumentNullException>(() => Result.Fail(null));
        }

        [Fact]
        public void TypedResultThrowsErrorWithNullFailures()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Fail<string>((List<IFailure>) null));
            Assert.Throws<ArgumentNullException>(() => Result.Fail<string>(null));
        }

        [Fact]
        public void ResultThrowsErrorWithEmptyFailures()
        {
            Assert.Throws<ArgumentException>(() => Result.Fail(new List<IFailure>()));
            Assert.Throws<ArgumentException>(() => Result.Fail());
        }

        [Fact]
        public void TypedResultThrowsErrorWithEmptyFailures()
        {
            Assert.Throws<ArgumentException>(() => Result.Fail<string>(new List<IFailure>()));
            Assert.Throws<ArgumentException>(() => Result.Fail<string>());
        }

        [Fact]
        public void TypedResultThrowsErrorWithEmptySuccess()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Ok((string) null));
        }

        [Fact]
        public void TypedResultAccessingFailedResultValueThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var unused = Result.Fail<object>(new Failure("some error")).Value;
            });
        }

        [Fact]
        public void CombineWithFailuresGivesFailures()
        {
            var result = Result.Combine(
                Result.Ok(),
                Result.Fail(new Failure("oops")),
                Result.Fail<int>(new Failure("oops")),
                Result.Fail<string>(new Failure("oops")),
                Result.Ok(1),
                Result.Ok("")
            );

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(3, result.Failures.Count());
        }

        [Fact]
        public void CombineWithNoFailuresGivesSuccess()
        {
            var result = Result.Combine(
                Result.Ok(),
                Result.Ok(1),
                Result.Ok("")
            );

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public async Task CombineAsyncWithFailuresGivesFailures()
        {
            var results = new List<IResult>
                {
                    Result.Ok(),
                    Result.Fail(new Failure("oops")),
                    Result.Fail<int>(new Failure("oops")),
                    Result.Fail<string>(new Failure("oops")),
                    Result.Ok(1),
                    Result.Ok("")
                }
                .Select(x => Task.FromResult(x));

            var result = await Result.CombineAsync(results.ToArray());

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(3, result.Failures.Count());
        }

        [Fact]
        public async Task CombineAsyncWithNoFailuresGivesSuccess()
        {
            var results = new List<IResult>
                {
                    Result.Ok(),
                    Result.Ok(1),
                    Result.Ok("")
                }
                .Select(x => Task.FromResult(x));

            var result = await Result.CombineAsync(results.ToArray());

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void ImplicitConversionFromTypedToUntypedProvidesCorrectVariant()
        {
            IResult source1 = Result.Ok("good");
            Assert.True(source1.IsSuccess);

            IResult source2 = Result.Fail<string>(new Failure("oops"));
            Assert.True(source2.IsFailed);
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
        public void TypedResultIsSuccess()
        {
            var result = Result.Ok("success");

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
            Assert.Equal("success", result.Value);
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
            var result = Result.Fail(new List<IFailure>
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
        public void ResultIsFailedFailureConstructor()
        {
            var result = Result.Fail(new Failure("Some error", "someHandle"));

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.Single().Message);
            Assert.Equal("someHandle", result.Failures.Single().UiHandle);
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
            var result = Result.Fail<string>(new List<IFailure>
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
        public void TypedResultIsFailedFailureConstructor()
        {
            var result = Result.Fail<string>(new Failure("Some error", "someHandle"));

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.Single().Message);
            Assert.Equal("someHandle", result.Failures.Single().UiHandle);
        }
    }
}
