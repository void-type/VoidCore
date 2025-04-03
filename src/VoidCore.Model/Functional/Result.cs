namespace VoidCore.Model.Functional;

/// <summary>
/// The result of a fallible operation that does not return a value on success. Generally used with CQRS Commands or
/// other void fallible operations. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public sealed class Result : ResultAbstract
{
    private Result() { }

    private Result(IEnumerable<IFailure> failures) : base(failures) { }

    /// <summary>
    /// Combine an array of results. If any have failed, this will return a new aggregate failed result. If none have
    /// failed, this will return a successful result. The returned result has no type.
    /// </summary>
    /// <param name="results">Results to combine</param>
    /// <returns>A new result</returns>
    public static IResult Combine(params IResult[] results)
    {
        return Combine(results.AsEnumerable());
    }

    /// <summary>
    /// Combine an IEnumerable of results. If any have failed, this will return a new aggregate failed result. If none have
    /// failed, this will return a successful result. The returned result has no type.
    /// </summary>
    /// <param name="results">Results to combine</param>
    /// <returns>A new result</returns>
    public static IResult Combine(IEnumerable<IResult> results)
    {
        var failures = results
            .Where(result => result.IsFailed)
            .SelectMany(result => result.Failures)
            .ToArray();

        return failures.Length > 0 ? Fail(failures) : Ok();
    }

    /// <summary>
    /// Combine an array of asynchronous results. If any have failed, this will return a new aggregate failed result.
    /// If none have failed, this will return a successful result. The returned result has no type.
    /// </summary>
    /// <param name="tasks">Task of IResult to combine</param>
    /// <returns>A new result</returns>
    public static Task<IResult> CombineAsync(params Task<IResult>[] tasks)
    {
        return CombineAsync(tasks.AsEnumerable());
    }

    /// <summary>
    /// Combine an array of asynchronous results. If any have failed, this will return a new aggregate failed result.
    /// If none have failed, this will return a successful result. The returned result has no type.
    /// </summary>
    /// <param name="tasks">Task of IResult to combine</param>
    /// <returns>A new result</returns>
    public static async Task<IResult> CombineAsync(IEnumerable<Task<IResult>> tasks)
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);

        return Combine(results);
    }

    /// <summary>
    /// Create a new failed result with a set a failures.
    /// </summary>
    /// <param name="failures">A set of failures</param>
    /// <returns>A new result</returns>
    public static IResult Fail(params IFailure[] failures)
    {
        return Fail(failures.AsEnumerable());
    }

    /// <summary>
    /// Create a new typed failed result with a set a failures.
    /// </summary>
    /// <param name="failures">A set of failures</param>
    /// <returns>A new result</returns>
    public static IResult<T> Fail<T>(params IFailure[] failures)
    {
        return Fail<T>(failures.AsEnumerable());
    }

    /// <summary>
    /// Create a new failed result with a set a failures.
    /// </summary>
    /// <param name="failures">A set of failures</param>
    /// <returns>A new result</returns>
    public static IResult Fail(IEnumerable<IFailure> failures)
    {
        return new Result(failures);
    }

    /// <summary>
    /// Create a new typed failed result with a set a failures.
    /// </summary>
    /// <param name="failures">A set of failures</param>
    /// <returns>A new result</returns>
    public static IResult<T> Fail<T>(IEnumerable<IFailure> failures)
    {
        return new Result<T>(failures);
    }

    /// <summary>
    /// Create a new successful untyped result.
    /// </summary>
    /// <returns>A new result</returns>
    public static IResult Ok()
    {
        return new Result();
    }

    /// <summary>
    /// Create a new successful typed result with a value.
    /// </summary>
    /// <param name="value">The result value</param>
    /// <typeparam name="T">The type of success value</typeparam>
    /// <returns>A new result</returns>
    public static IResult<T> Ok<T>(T value)
    {
        return new Result<T>(value);
    }
}
