using System;
using System.Collections.Generic;
using VoidCore.Model.Functional;

namespace VoidCore.Model.RuleValidator;

/// <summary>
/// Builds a rule for validating an request.
/// </summary>
/// <typeparam name="T">The type of request to validate.</typeparam>
internal class RuleBuilder<T> : IRuleBuilder<T>
{
    private readonly Func<T, IFailure> _failureBuilder;
    private readonly List<Func<T, bool>> _invalidConditions = new();
    private readonly List<Func<T, bool>> _suppressConditions = new();

    /// <summary>
    /// Construct a new rule and underlying validation error to throw when violations are detected.
    /// </summary>
    /// <param name="failureBuilder">A function that builds a custom IFailure to return if the rule fails.</param>
    internal RuleBuilder(Func<T, IFailure> failureBuilder)
    {
        _failureBuilder = failureBuilder;
    }

    /// <inheritdoc/>
    public IRuleBuilder<T> ExceptWhen(Func<T, bool> suppressCondition)
    {
        _suppressConditions.Add(suppressCondition);
        return this;
    }

    /// <inheritdoc/>
    public IRuleBuilder<T> InvalidWhen(Func<T, bool> invalidCondition)
    {
        _invalidConditions.Add(invalidCondition);
        return this;
    }

    /// <summary>
    /// Build the rule.
    /// </summary>
    internal Rule<T> Build()
    {
        return new Rule<T>(_failureBuilder, _invalidConditions, _suppressConditions);
    }
}
