﻿namespace VoidCore.Model.Functional;

/// <summary>
/// Base class for Result classes. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
/// </summary>
public abstract class ResultAbstract : IResult
{
    private readonly ResultInternal _internalResult;

    /// <summary>
    /// Construct a failed result
    /// </summary>
    protected ResultAbstract(IEnumerable<IFailure> failures)
    {
        _internalResult = new ResultInternal(failures);
    }

    /// <summary>
    /// Construct a successful result
    /// </summary>
    protected ResultAbstract()
    {
        _internalResult = new ResultInternal();
    }

    /// <inheritdoc/>
    public IEnumerable<IFailure> Failures => _internalResult.Failures;

    /// <inheritdoc/>
    public bool IsFailed => _internalResult.IsFailed;

    /// <inheritdoc/>
    public bool IsSuccess => _internalResult.IsSuccess;
}
