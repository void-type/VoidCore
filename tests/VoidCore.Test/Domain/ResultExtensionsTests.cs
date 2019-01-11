using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class ResultExtensionsTests
    {
        [Fact]
        public void CombineWithFailuresGivesFailures()
        {
            var result = new List<IResult>()
                {
                    Result.Ok(),
                        Result.Fail("oops"),
                        Result.Fail("oops"),
                        Result.Ok()
                }
                .Combine();

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void CombineWithNoFailuresGivesSuccess()
        {
            var results = new List<IResult>() { Result.Ok(), Result.Ok() };
            var result = results.Combine();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void TypedCombineWithFailuresGivesFailures()
        {
            var result = new List<IResult<string>>()
                {
                    Result.Ok(""),
                        Result.Fail<string>(""),
                        Result.Fail<string>(""),
                        Result.Ok("")
                }
                .Combine();

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void TypedCombineWithNoFailuresGivesSuccess()
        {
            var results = new List<IResult<string>>() { Result.Ok(""), Result.Ok("") };
            var result = results.Combine();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void TeeOnSuccessTests()
        {
            var tick = 1;
            var okResult = Result.Ok();
            var failResult = Result.Fail("something happened");

            okResult
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++);

            Assert.Equal(4, tick);

            failResult
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public void TypedTeeOnSuccessTests()
        {
            var tick = 1;
            var okResult = Result.Ok(2);
            var failResult = Result.Fail<int>("something happened");

            okResult
                .TeeOnSuccess(r => tick += r)
                .TeeOnSuccess(r => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(r => tick += r);

            Assert.Equal(7, tick);

            failResult
                .TeeOnSuccess(r => tick += r)
                .TeeOnSuccess(r => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(r => tick += r);

            Assert.Equal(7, tick);
        }

        [Fact]
        public void TeeOnFailureTests()
        {
            var tick = 1;
            var okResult = Result.Ok();
            var failResult = Result.Fail("something happened");

            okResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++);

            Assert.Equal(1, tick);

            failResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public void TypedTeeOnFailureTests()
        {
            var tick = 1;
            var okResult = Result.Ok(2);
            var failResult = Result.Fail<int>("something happened");

            okResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++);

            Assert.Equal(1, tick);

            failResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public void SelectTests()
        {
            var okResult = Result.Ok();
            var failResult = Result.Fail("something happened");

            var newOkResult = okResult
                .Select(() => "new value");

            Assert.True(newOkResult.IsSuccess);
            Assert.Equal("new value", newOkResult.Value);

            var newFailResult = failResult
                .Select(() => "new value");

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("something happened", newFailResult.Failures.First().Message);
        }

        [Fact]
        public void TypedSelectTests()
        {
            var okResult = Result.Ok(2);
            var failResult = Result.Fail<int>("something happened");

            var newOkResult = okResult
                .Select(r => "new value" + r);

            Assert.True(newOkResult.IsSuccess);
            Assert.Equal("new value2", newOkResult.Value);

            var newFailResult = failResult
                .Select(r => "new value" + r);

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("something happened", newFailResult.Failures.First().Message);
        }
    }
}
