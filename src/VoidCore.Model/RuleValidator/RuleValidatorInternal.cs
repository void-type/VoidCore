using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace VoidCore.Model.RuleValidator;

/// <summary>
/// Internal logic for a custom rule-based request validator.
/// </summary>
/// <typeparam name="T">The type of entities to validate</typeparam>
public class RuleValidatorInternal<T> : IRequestValidator<T>
{
    private readonly List<RuleBuilder<T>> _ruleBuilders = [];

    internal RuleValidatorInternal()
    {
    }

    /// <inheritdoc/>
    public IResult<T> Validate(T request)
    {
        return _ruleBuilders
            .Select(builder => builder.Build().Run(request))
            .Combine()
            .Select(() => request);
    }

    /// <summary>
    /// Create a new rule for this request.
    /// </summary>
    /// <param name="failureBuilder">A builder function for the failure to give upon invalid request.</param>
    /// <returns>A rule builder to continue building this rule</returns>
    public IRuleBuilder<T> CreateRule(Func<T, IFailure> failureBuilder)
    {
        var rule = new RuleBuilder<T>(failureBuilder);
        _ruleBuilders.Add(rule);
        return rule;
    }

    /// <summary>
    /// Create a new rule for this request.
    /// </summary>
    /// <param name="failure">The failure to give upon invalid request.</param>
    /// <returns>A rule builder to continue building this rule</returns>
    public IRuleBuilder<T> CreateRule(IFailure failure)
    {
        return CreateRule(_ => failure);
    }

    /// <summary>
    /// Create a new rule for this request.
    /// </summary>
    /// <param name="errorMessage">UI friendly error message</param>
    /// <param name="uiHandle">The entity property name that is in error. Can be mapped to a field on the view</param>
    /// <returns>A rule builder to continue building this rule</returns>
    public IRuleBuilder<T> CreateRule(string errorMessage, string? uiHandle = null)
    {
        return CreateRule(_ => new Failure(errorMessage, uiHandle));
    }
}
