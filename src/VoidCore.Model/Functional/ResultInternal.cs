using VoidCore.Model.Guards;

namespace VoidCore.Model.Functional;

/// <summary>
/// This class holds the internal logic for the Result abstract class and its inheritors. InternalResult should not
/// be directly accessed outside of its Result wrapper. This class shares its constructor logic with inheritors.
/// Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
internal sealed class ResultInternal
{
    internal ResultInternal(IEnumerable<IFailure> failures)
    {
        // Copy once: enumerating a lazy sequence twice would run it twice, and the result must not change if the caller's array does.
        var failuresArray = failures.EnsureNotNull().ToArray();
        failuresArray.EnsureNotNullOrEmpty();

        Failures = failuresArray;
        IsFailed = true;
    }

    /// <summary>
    /// Construct a successful result
    /// </summary>
    internal ResultInternal()
    {
        IsFailed = false;
    }

    public IEnumerable<IFailure> Failures { get; } = Array.Empty<IFailure>();
    public bool IsFailed { get; }
    public bool IsSuccess => !IsFailed;
}
