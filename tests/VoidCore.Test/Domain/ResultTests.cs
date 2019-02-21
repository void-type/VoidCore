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
        public void ResultThrowsErrorWithEmptyFailures()
        {
            Assert.Throws<ArgumentException>(() => Result.Fail(new IFailure[0]));
            Assert.Throws<ArgumentException>(() => Result.Fail(new List<IFailure>()));
        }

        [Fact]
        public void ResultThrowsErrorWithNullFailures()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Fail((IFailure[]) null));
            Assert.Throws<ArgumentNullException>(() => Result.Fail((IFailure) null));
        }

        [Fact]
        public void TypedResultAccessingFailedResultValueThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var unused = Result.Fail<object>("some error").Value;
            });
        }

        [Fact]
        public void TypedResultThrowsErrorWithEmptyFailures()
        {
            Assert.Throws<ArgumentException>(() => Result.Fail<string>(new IFailure[0]));
        }

        [Fact]
        public void TypedResultThrowsErrorWithEmptySuccess()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Ok((string) null));
        }

        [Fact]
        public void TypedResultThrowsErrorWithNullFailures()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Fail<string>((IFailure[]) null));
            Assert.Throws<ArgumentNullException>(() => Result.Fail<string>((IFailure) null));
        }

        [Fact]
        public void CombineWithFailuresGivesFailures()
        {
            var result = Result.Combine(
                Result.Ok(),
                Result.Fail("oops"),
                Result.Fail("oops"),
                Result.Ok()
            );

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void CombineWithNoFailuresGivesSuccess()
        {
            var result = Result.Combine(
                Result.Ok(),
                Result.Ok()
            );

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void TypedCombineWithFailuresGivesFailures()
        {
            var result = Result.Combine(
                Result.Ok(""),
                Result.Fail<string>(""),
                Result.Fail<string>(""),
                Result.Ok("")
            );

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void TypedCombineWithNoFailuresGivesSuccess()
        {
            var result = Result.Combine(
                Result.Ok(""),
                Result.Ok("")
            );

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public async Task CombineAsyncWithFailuresGivesFailures()
        {
            var result = await Result.CombineAsync(
                SuccessfulTask(),
                FailedTask(),
                FailedTask(),
                SuccessfulTask()
            );

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public async Task CombineAsyncWithNoFailuresGivesSuccess()
        {
            var result = await Result.CombineAsync(
                SuccessfulTask(),
                SuccessfulTask()
            );

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void ImplicitConversionFromTypedToUntypedProvidesCorrectVariant()
        {
            IResult source1 = Result.Ok("good");
            Assert.True(source1.IsSuccess);

            IResult source2 = Result.Fail<string>("oops");
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
            var result = Result.Fail("Some error", "someHandle");

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
            var result = Result.Fail<string>("Some error", "someHandle");

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.NotEmpty(result.Failures);
            Assert.Equal("Some error", result.Failures.Single().Message);
            Assert.Equal("someHandle", result.Failures.Single().UiHandle);
        }

        private async Task<IResult> FailedTask()
        {
            await Task.Delay(1);
            return Result.Fail("oops");
        }

        private async Task<IResult> SuccessfulTask()
        {
            await Task.Delay(1);
            return Result.Ok();
        }
    }
}
