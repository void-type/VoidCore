using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class ResultExtensionsTests
    {
        [Fact]
        public void CombineWithNoFailuresGivesSuccess()
        {
            var result = new List<IResult>
            {
                Result.Ok(),
                Result.Ok(1),
                Result.Ok("")
            }.Combine();

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public void CombineWithFailuresGivesFailures()
        {
            var result = new List<IResult>
            {
                Result.Ok(),
                Result.Fail(new Failure("oops")),
                Result.Fail<int>(new Failure("oops")),
                Result.Fail<string>(new Failure("oops")),
                Result.Ok(1),
                Result.Ok("")
            }.Combine();

            Assert.True(result.IsFailed);
            Assert.Equal(3, result.Failures.Count());
        }

        [Fact]
        public async Task CombineAsyncWithNoFailuresGivesSuccess()
        {
            var result = await new List<IResult>
                {
                    Result.Ok(),
                    Result.Ok(1),
                    Result.Ok("")
                }
                .Select(x => Task.Run(() => x))
                .CombineAsync();

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Failures);
        }

        [Fact]
        public async Task CombineAsyncWithFailuresGivesFailures()
        {
            var result = await new List<IResult>
                {
                    Result.Ok(),
                    Result.Fail(new Failure("oops")),
                    Result.Fail<int>(new Failure("oops")),
                    Result.Fail<string>(new Failure("oops")),
                    Result.Ok(1),
                    Result.Ok("")
                }
                .Select(x => Task.Run(() => x))
                .CombineAsync();

            Assert.True(result.IsFailed);
            Assert.Equal(3, result.Failures.Count());
        }

        [Fact]
        public void SelectTests()
        {
            var okResult = Result.Ok();

            var newOkResult = okResult
                .Select(() => "new value")
                .Select(r => "new value")
                .Select(() => "new value");

            Assert.True(newOkResult.IsSuccess);
            Assert.Equal("new value", newOkResult.Value);

            var failResult = Result.Fail(new Failure("oops"));

            var newFailResult = failResult
                .Select(() => "new value")
                .Select(r => "new value")
                .Select(() => "new value");

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("oops", newFailResult.Failures.First().Message);
        }

        [Fact]
        public async Task SelectAsyncTests()
        {
            var t = new TestTransformerService();

            var newOkResult = await Result.Ok()
                .SelectAsync(() => t.TransformAsync(t.Start, 1))
                .SelectAsync(r => t.Transform(r, 2))
                .SelectAsync(r => t.TransformAsync(r, 3));

            Assert.True(newOkResult.IsSuccess);
            Assert.Equal("Hello World!!!", newOkResult.Value);

            newOkResult = await Task.Run(() => Result.Ok())
                .SelectAsync(() => t.Transform(t.Start, 4));

            Assert.True(newOkResult.IsSuccess);
            Assert.Equal("Hello World!", newOkResult.Value);

            newOkResult = await Task.Run(() => Result.Ok())
                .SelectAsync(() => t.TransformAsync(t.Start, 5))
                .SelectAsync(r => t.TransformAsync(r, 6));

            Assert.True(newOkResult.IsSuccess);
            Assert.Equal("Hello World!!", newOkResult.Value);

            var newFailResult = await Result.Fail(new Failure("oops"))
                .SelectAsync(() => t.TransformAsync(t.Start, 1))
                .SelectAsync(r => t.Transform(r, 2))
                .SelectAsync(r => t.TransformAsync(r, 3));

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("oops", newFailResult.Failures.First().Message);
        }

        [Fact]
        public void ThenTests()
        {
            var newOkResult = Result.Ok()
                .Then(() => Result.Ok())
                .Then(() => Result.Ok(""))
                .Then(r => Result.Ok(""))
                .Then(r => Result.Ok())
                .Then(() => Result.Ok(""))
                .Then(() => Result.Ok(""))
                .Then(r => Result.Ok(2))
                .Then(() => Result.Ok(2));

            Assert.True(newOkResult.IsSuccess);
            Assert.Equal(2, newOkResult.Value);

            var newFailResult = Result.Fail<int>(new Failure("oops"))
                .Then(() => Result.Ok())
                .Then(() => Result.Ok(""))
                .Then(r => Result.Ok(""))
                .Then(r => Result.Ok())
                .Then(() => Result.Ok(""))
                .Then(() => Result.Ok(""))
                .Then(r => Result.Ok(2))
                .Then(() => Result.Ok(2));

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("oops", newFailResult.Failures.First().Message);
        }

        [Fact]
        public async Task ThenAsyncTests()
        {
            var t = new TestTransformerService();

            var newOkResult = await Result.Ok()
                .ThenAsync(() => t.GetResultAsync(1))
                .ThenAsync(() => t.GetResult(2))
                .ThenAsync(() => t.GetResultAsync(3))
                .ThenAsync(() => t.GetResult("", 4))
                .ThenAsync(r => t.GetResult("", 5))
                .ThenAsync(r => t.GetResult("", 6))
                .ThenAsync(r => t.GetResult("", 7))
                .ThenAsync(r => t.GetResultAsync(8))
                .ThenAsync(() => t.GetResultAsync(2, 9))
                .ThenAsync(r => t.GetResult(2, 10));

            Assert.True(newOkResult.IsSuccess);
            Assert.Equal(2, newOkResult.Value);

            t = new TestTransformerService();

            var newFailResult = await Result.Fail<int>(new Failure("oops"))
                .ThenAsync(r => t.GetResultAsync("", 1))
                .ThenAsync(r => t.GetResult("", 2))
                .ThenAsync(r => t.GetResultAsync("", 2))
                .ThenAsync(r => t.GetResultAsync(3))
                .ThenAsync(() => t.GetResultAsync(4))
                .ThenAsync(() => t.GetResult("", 5))
                .ThenAsync(r => t.GetResult("", 6))
                .ThenAsync(r => t.GetResultAsync(7))
                .ThenAsync(() => t.GetResultAsync(2, 8))
                .ThenAsync(r => t.GetResult(9));

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("oops", newFailResult.Failures.First().Message);
        }
    }
}
