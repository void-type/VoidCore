using VoidCore.Model.Functional;
using Xunit;

namespace VoidCore.Test.Model.Functional;

public class MaybeExtensionsTeeTests
{
    [Fact]
    public void TeeOnSuccess_runs_when_maybe_has_value()
    {
        var tick = 1;
        var maybe = Maybe.From(2)
            .TeeOnSuccess(r => tick += r)
            .TeeOnSuccess(r => tick++)
            .TeeOnSuccess(() => tick++)
            .TeeOnSuccess(r => tick += r);

        Assert.Equal(7, tick);
    }

    [Fact]
    public void TeeOnSuccess_doesnt_run_when_maybe_has_no_value()
    {
        var tick = 1;
        var maybe = Maybe.None<int>()
            .TeeOnSuccess(r => tick += r)
            .TeeOnSuccess(r => tick++)
            .TeeOnSuccess(() => tick++)
            .TeeOnSuccess(r => tick += r);

        Assert.Equal(1, tick);
    }

    [Fact]
    public void TeeOnFailure_runs_when_maybe_has_no_value()
    {
        var tick = 1;
        var maybe = Maybe.None<string>()
            .TeeOnFailure(() => tick++)
            .TeeOnFailure(() => tick++)
            .TeeOnFailure(() => tick++);

        Assert.Equal(4, tick);
    }

    [Fact]
    public void TeeOnFailure_doesnt_run_when_maybe_has_value()
    {
        var tick = 1;
        var maybe = Maybe.From(2)
            .TeeOnFailure(() => tick++)
            .TeeOnFailure(() => tick++)
            .TeeOnFailure(() => tick++);

        Assert.Equal(1, tick);
    }

    [Fact]
    public async Task TeeAsync_tests()
    {
        var p = new TestPerformerService();

        var maybe = await Task.FromResult(Maybe.From(string.Empty))
            .TeeOnSuccessAsync(r => p.DoAsync(1))
            .TeeOnFailureAsync(() => p.DoAsync(1))
            .TeeOnFailureAsync(() => p.Do(2))
            .TeeOnSuccessAsync(() => p.Do(2))
            .TeeOnFailureAsync(() => p.Do(3))
            .TeeOnSuccessAsync(r => p.Do(3))
            .TeeOnFailureAsync(() => p.DoAsync(4))
            .TeeOnSuccessAsync(() => p.DoAsync(4))
            .TeeOnFailureAsync(() => p.DoAsync(5))
            .TeeOnSuccessAsync(r => p.DoAsync(5));

        Assert.True(maybe.HasValue);

        p.Reset();

        var maybeNone = await Task.FromResult(Maybe.None<int>())
            .TeeOnSuccessAsync(r => p.DoAsync(1))
            .TeeOnFailureAsync(() => p.DoAsync(1))
            .TeeOnFailureAsync(() => p.Do(2))
            .TeeOnSuccessAsync(() => p.Do(2))
            .TeeOnFailureAsync(() => p.Do(3))
            .TeeOnSuccessAsync(r => p.Do(3))
            .TeeOnFailureAsync(() => p.DoAsync(4))
            .TeeOnSuccessAsync(() => p.DoAsync(4))
            .TeeOnFailureAsync(() => p.DoAsync(5))
            .TeeOnSuccessAsync(r => p.DoAsync(5));

        Assert.True(maybeNone.HasNoValue);
    }
}
