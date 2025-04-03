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
        failures.EnsureNotNullOrEmpty();
        Failures = failures as IFailure[] ?? failures.ToArray();
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
