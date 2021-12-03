using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Model.Functional;
using Xunit;

namespace VoidCore.Test.Model.Functional;

public class ResultExtensionsTests
{
    [Fact]
    public void Combine_with_no_failures_is_success()
    {
        var result = new List<IResult>
            {
                Result.Ok(),
                Result.Ok(1),
                Result.Ok(string.Empty)
            }.Combine();

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Failures);
    }

    [Fact]
    public void Combine_with_failures_is_failed()
    {
        var result = new List<IResult>
            {
                Result.Ok(),
                Result.Fail(new Failure("oops")),
                Result.Fail<int>(new Failure("oops")),
                Result.Fail<string>(new Failure("oops")),
                Result.Ok(1),
                Result.Ok(string.Empty)
            }.Combine();

        Assert.True(result.IsFailed);
        Assert.Equal(3, result.Failures.Count());
    }

    [Fact]
    public async Task CombineAsync_with_no_failures_is_success()
    {
        var result = await new List<IResult>
                {
                    Result.Ok(),
                    Result.Ok(1),
                    Result.Ok(string.Empty)
                }
            .Select(Task.FromResult)
            .CombineAsync();

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Failures);
    }

    [Fact]
    public async Task CombineAsync_with_failures_is_failed()
    {
        var result = await new List<IResult>
                {
                    Result.Ok(),
                    Result.Fail(new Failure("oops")),
                    Result.Fail<int>(new Failure("oops")),
                    Result.Fail<string>(new Failure("oops")),
                    Result.Ok(1),
                    Result.Ok(string.Empty)
                }
            .Select(Task.FromResult)
            .CombineAsync();

        Assert.True(result.IsFailed);
        Assert.Equal(3, result.Failures.Count());
    }

    [Fact]
    public void Select_from_ok_returns_transformation()
    {
        var okResult = Result.Ok();

        var newOkResult = okResult
            .Select(() => "new value")
            .Select(r => r + "!");

        Assert.True(newOkResult.IsSuccess);
        Assert.Equal("new value!", newOkResult.Value);
    }

    [Fact]
    public void Select_from_fail_returns_failure()
    {
        var failResult = Result.Fail(new Failure("oops"));

        var newFailResult = failResult
            .Select(() => "new value")
            .Select(r => "new value");

        Assert.True(newFailResult.IsFailed);
        Assert.Equal("oops", newFailResult.Failures.First().Message);
    }

    [Fact]
    public async Task SelectAsync_from_ok_returns_transformation()
    {
        var t = new TestTransformerService();

        var newOkResult = await Result.Ok()
            .SelectAsync(() => t.TransformAsync(TestTransformerService.Start, 1))
            .SelectAsync(r => t.Transform(r, 2))
            .SelectAsync(r => t.TransformAsync(r, 3));

        Assert.True(newOkResult.IsSuccess);
        Assert.Equal("Hello World!!!", newOkResult.Value);

        newOkResult = await Task.FromResult(Result.Ok())
            .SelectAsync(() => t.Transform(TestTransformerService.Start, 4));

        Assert.True(newOkResult.IsSuccess);
        Assert.Equal("Hello World!", newOkResult.Value);

        newOkResult = await Task.FromResult(Result.Ok())
            .SelectAsync(() => t.TransformAsync(TestTransformerService.Start, 5))
            .SelectAsync(r => t.TransformAsync(r, 6));

        Assert.True(newOkResult.IsSuccess);
        Assert.Equal("Hello World!!", newOkResult.Value);
    }

    [Fact]
    public async Task SelectAsync_from_failure_returns_failure()
    {
        var t = new TestTransformerService();

        var newFailResult = await Result.Fail(new Failure("oops"))
            .SelectAsync(() => t.TransformAsync(TestTransformerService.Start, 1))
            .SelectAsync(r => t.Transform(r, 2))
            .SelectAsync(r => t.TransformAsync(r, 3));

        Assert.True(newFailResult.IsFailed);
        Assert.Equal("oops", newFailResult.Failures.First().Message);
    }

    [Fact]
    public void Then_from_ok_runs_function()
    {
        var newOkResult = Result.Ok()
            .Then(Result.Ok)
            .Then(() => Result.Ok(string.Empty))
            .Then(r => Result.Ok(string.Empty))
            .Then(r => Result.Ok())
            .Then(() => Result.Ok(string.Empty))
            .Then(() => Result.Ok(string.Empty))
            .Then(r => Result.Ok(2))
            .Then(() => Result.Ok(2));

        Assert.True(newOkResult.IsSuccess);
        Assert.Equal(2, newOkResult.Value);
    }

    [Fact]
    public void Then_from_failure_doesnt_run_function()
    {
        var newFailResult = Result.Fail<int>(new Failure("oops"))
            .Then(() => Result.Ok())
            .Then(() => Result.Ok(string.Empty))
            .Then(r => Result.Ok(string.Empty))
            .Then(r => Result.Ok())
            .Then(() => Result.Ok(string.Empty))
            .Then(() => Result.Ok(string.Empty))
            .Then(r => Result.Ok(2))
            .Then(() => Result.Ok(2));

        Assert.True(newFailResult.IsFailed);
        Assert.Equal("oops", newFailResult.Failures.First().Message);
    }

    [Fact]
    public async Task ThenAsync_from_ok_runs_function()
    {
        var t = new TestTransformerService();

        var newOkResult = await Result.Ok()
            .ThenAsync(() => t.GetResultAsync(1))
            .ThenAsync(() => t.GetResult(2))
            .ThenAsync(() => t.GetResultAsync(3))
            .ThenAsync(() => t.GetResult(string.Empty, 4))
            .ThenAsync(r => t.GetResult(string.Empty, 5))
            .ThenAsync(r => t.GetResult(string.Empty, 6))
            .ThenAsync(r => t.GetResult(string.Empty, 7))
            .ThenAsync(r => t.GetResultAsync(8))
            .ThenAsync(() => t.GetResultAsync(2, 9))
            .ThenAsync(r => t.GetResult(2, 10));

        Assert.True(newOkResult.IsSuccess);
        Assert.Equal(2, newOkResult.Value);
    }

    [Fact]
    public async Task ThenAsync_from_failure_doesnt_run_function()
    {
        var t = new TestTransformerService();

        var newFailResult = await Result.Fail<int>(new Failure("oops"))
            .ThenAsync(r => t.GetResultAsync(string.Empty, 1))
            .ThenAsync(r => t.GetResult(string.Empty, 2))
            .ThenAsync(r => t.GetResultAsync(string.Empty, 2))
            .ThenAsync(r => t.GetResultAsync(3))
            .ThenAsync(() => t.GetResultAsync(4))
            .ThenAsync(() => t.GetResult(string.Empty, 5))
            .ThenAsync(r => t.GetResult(string.Empty, 6))
            .ThenAsync(r => t.GetResultAsync(7))
            .ThenAsync(() => t.GetResultAsync(2, 8))
            .ThenAsync(r => t.GetResult(9));

        Assert.True(newFailResult.IsFailed);
        Assert.Equal("oops", newFailResult.Failures.First().Message);
    }
}
