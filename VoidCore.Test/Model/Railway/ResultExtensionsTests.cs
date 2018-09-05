using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Railway;
using Xunit;

namespace VoidCore.Test.Model.Railway
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
            var results = new List<Result>() { Result.Ok(), Result.Fail("oops"), Result.Fail("oops"), Result.Ok() }.ToArray();
            var result = results.Combine();

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }

        [Fact]
        public void TypedCombineWithFailuresGivesFailures()
        {
            var results = new List<Result<string>>() { Result.Ok(""), Result.Fail<string>(""), Result.Fail<string>(""), Result.Ok("") }.ToArray();
            var result = results.Combine();

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailed);
            Assert.Equal(2, result.Failures.Count());
        }
    }
}
