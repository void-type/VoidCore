using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.DomainEvents;
using Xunit;

namespace VoidCore.Test.Model.DomainEvents
{
    public class ResultExtensionsTests
    {
        [Fact]
        public void CombineWithNoFailuresGivesSuccess()
        {
            var results = new List<Result>() { Result.Ok(), Result.Ok() };
            var result = results.Combine();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void TypedCombineWithNoFailuresGivesSuccess()
        {
            var results = new List<Result<string>>() { Result.Ok(""), Result.Ok("") };
            var result = results.Combine();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailed);
            Assert.Empty(result.Failures);
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
        public void TypedCombineWithFailuresGivesFailures()
        {
            var result = new List<Result<string>>() { Result.Ok(""), Result.Fail<string>(""), Result.Fail<string>(""), Result.Ok("") }
                .Combine();

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }
    }
}