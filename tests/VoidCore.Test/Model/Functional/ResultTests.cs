using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Model.Functional;
using Xunit;

namespace VoidCore.Test.Model.Functional;

public class ResultTests
{
    [Fact]
    public void Creating_Result_with_null_failures_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Fail(null));
    }

    [Fact]
    public void Creating_typed_Result_with_null_failures_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Fail<string>(null));
    }

    [Fact]
    public void Creating_Result_with_empty_failures_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Result.Fail());
    }

    [Fact]
    public void Creating_typed_Result_with_empty_failures_throws_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Result.Fail<string>());
    }

    [Fact]
    public void Creating_typed_Result_with_empty_success_throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Ok((string)null));
    }

    [Fact]
    public void Accessing_failed_typed_Result_value_throws_InvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var unused = Result.Fail<object>(new Failure("some error")).Value;
        });
    }

    [Fact]
    public void Combine_with_any_failures_gives_a_failed_result()
    {
        var result = Result.Combine(
            Result.Ok(),
            Result.Fail(new Failure("oops")),
            Result.Fail<int>(new Failure("oops")),
            Result.Fail<string>(new Failure("oops")),
            Result.Ok(1),
            Result.Ok(string.Empty)
        );

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);
        Assert.Equal(3, result.Failures.Count());
    }

    [Fact]
    public void Combine_with_no_failures_gives_a_success_result()
    {
        var result = Result.Combine(
            Result.Ok(),
            Result.Ok(1),
            Result.Ok(string.Empty)
        );

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);
        Assert.Empty(result.Failures);
    }

    [Fact]
    public async Task CombineAsync_with_any_failures_gives_a_failed_result()
    {
        var results = new List<IResult>
                {
                    Result.Ok(),
                    Result.Fail(new Failure("oops")),
                    Result.Fail<int>(new Failure("oops")),
                    Result.Fail<string>(new Failure("oops")),
                    Result.Ok(1),
                    Result.Ok(string.Empty)
                }
            .Select(Task.FromResult);

        var result = await Result.CombineAsync(results.ToArray());

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);
        Assert.Equal(3, result.Failures.Count());
    }

    [Fact]
    public async Task CombineAsync_with_no_failures_gives_a_success_result()
    {
        var results = new List<IResult>
                {
                    Result.Ok(),
                    Result.Ok(1),
                    Result.Ok(string.Empty)
                }
            .Select(Task.FromResult);

        var result = await Result.CombineAsync(results.ToArray());

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);
        Assert.Empty(result.Failures);
    }

    [Fact]
    public void ImplicitConversion_from_typed_to_untyped_Result_doesnt_change_result_status()
    {
        IResult source1 = Result.Ok("good");
        Assert.True(source1.IsSuccess);

        IResult source2 = Result.Fail<string>(new Failure("oops"));
        Assert.True(source2.IsFailed);
    }

    [Fact]
    public void Result_is_success()
    {
        var result = Result.Ok();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);
        Assert.Empty(result.Failures);
    }

    [Fact]
    public void Result_is_failed_single_failure()
    {
        var result = Result.Fail(new Failure("Some error", "someHandle"));

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);
        Assert.NotEmpty(result.Failures);
        Assert.Equal("Some error", result.Failures.Single().Message);
        Assert.Equal("someHandle", result.Failures.Single().UiHandle);
    }

    [Fact]
    public void Result_is_failed_list_failures()
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
    public void Result_is_failed_params_failures()
    {
        var result = Result.Fail(
            new Failure("Some error", "someHandle"),
            new Failure("Some error", "someHandle"));

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);
        Assert.NotEmpty(result.Failures);
        Assert.Equal("Some error", result.Failures.First().Message);
        Assert.Equal("someHandle", result.Failures.First().UiHandle);
        Assert.Equal(2, result.Failures.Count());
    }

    [Fact]
    public void Typed_Result_is_success()
    {
        var result = Result.Ok("success");

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);
        Assert.Empty(result.Failures);
        Assert.Equal("success", result.Value);
    }

    [Fact]
    public void Typed_Result_is_failed_single_failure()
    {
        var result = Result.Fail<string>(new Failure("Some error", "someHandle"));

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);
        Assert.NotEmpty(result.Failures);
        Assert.Equal("Some error", result.Failures.Single().Message);
        Assert.Equal("someHandle", result.Failures.Single().UiHandle);
    }

    [Fact]
    public void Typed_Result_is_failed_list_failures()
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
    public void Typed_Result_is_failed_params_failures()
    {
        var result = Result.Fail<string>(
            new Failure("Some error", "someHandle"),
            new Failure("Some error", "someHandle"));

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);
        Assert.NotEmpty(result.Failures);
        Assert.Equal("Some error", result.Failures.First().Message);
        Assert.Equal("someHandle", result.Failures.First().UiHandle);
        Assert.Equal(2, result.Failures.Count());
    }
}
