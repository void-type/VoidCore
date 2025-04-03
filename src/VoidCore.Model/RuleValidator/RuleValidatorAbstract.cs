using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace VoidCore.Model.RuleValidator;

/// <summary>
/// Abstract for an immutable custom rule-based request validator.
/// </summary>
/// <typeparam name="T">The type of entities to validate</typeparam>
public abstract class RuleValidatorAbstract<T> : IRequestValidator<T>
{
    private readonly RuleValidatorInternal<T> _ruleValidatorInternal = new();

    /// <inheritdoc/>
    public IResult<T> Validate(T request)
    {
        return _ruleValidatorInternal.Validate(request);
    }

    /// <summary>
    /// Create a new rule for this request.
    /// </summary>
    /// <param name="failureBuilder">A builder function for the failure to give upon invalid request.</param>
    /// <returns>A rule builder to continue building this rule</returns>
    protected IRuleBuilder<T> CreateRule(Func<T, IFailure> failureBuilder)
    {
        return _ruleValidatorInternal.CreateRule(failureBuilder);
    }

    /// <summary>
    /// Create a new rule for this request.
    /// </summary>
    /// <param name="failure">The failure to give upon invalid request.</param>
    /// <returns>A rule builder to continue building this rule</returns>
    protected IRuleBuilder<T> CreateRule(IFailure failure)
    {
        return _ruleValidatorInternal.CreateRule(failure);
    }

    /// <summary>
    /// Create a new rule for this request.
    /// </summary>
    /// <param name="errorMessage">UI friendly error message</param>
    /// <param name="uiHandle">The entity property name that is in error. Can be mapped to a field on the view</param>
    /// <returns>A rule builder to continue building this rule</returns>
    protected IRuleBuilder<T> CreateRule(string errorMessage, string? uiHandle = null)
    {
        return _ruleValidatorInternal.CreateRule(errorMessage, uiHandle);
    }
}
