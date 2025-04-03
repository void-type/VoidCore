using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace VoidCore.Model.RuleValidator;

/// <summary>
/// Extensions to simplify validation.
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// Run a validator against the request.
    /// </summary>
    /// <typeparam name="T">The type of entities to validate</typeparam>
    /// <param name="request">The entity to validate</param>
    /// <param name="validator">The validator</param>
    /// <returns></returns>
    public static IResult<T> Validate<T>(this T request, IRequestValidator<T> validator)
    {
        return validator.Validate(request);
    }

    /// <summary>
    /// Build a validator and run it against a request.
    /// </summary>
    /// <typeparam name="T">The type of entities to validate</typeparam>
    /// <param name="request">The entity to validate</param>
    /// <param name="options">Validator builder function</param>
    /// <returns></returns>
    public static IResult<T> Validate<T>(this T request, Action<RuleValidatorInternal<T>> options)
    {
        var validator = new RuleValidatorInternal<T>();
        options(validator);
        return validator.Validate(request);
    }

    /// <summary>
    /// Create invalid condition to check selected property for string.IsNullOrWhiteSpace.
    /// </summary>
    /// <typeparam name="T">The type of entities to validate</typeparam>
    /// <param name="ruleBuilder">The rule builder</param>
    /// <param name="selector">Property selector</param>
    public static IRuleBuilder<T> InvalidWhenNullOrWhiteSpace<T>(this IRuleBuilder<T> ruleBuilder, Func<T, string> selector)
    {
        ruleBuilder.InvalidWhen(x => string.IsNullOrWhiteSpace(selector(x)));
        return ruleBuilder;
    }

    /// <summary>
    /// Create invalid condition to check selected property for string.IsNullOrEmpty.
    /// </summary>
    /// <typeparam name="T">The type of entities to validate</typeparam>
    /// <param name="ruleBuilder">The rule builder</param>
    /// <param name="selector">Property selector</param>
    public static IRuleBuilder<T> InvalidWhenNullOrEmpty<T>(this IRuleBuilder<T> ruleBuilder, Func<T, string> selector)
    {
        ruleBuilder.InvalidWhen(x => string.IsNullOrEmpty(selector(x)));
        return ruleBuilder;
    }

    /// <summary>
    /// Create invalid condition to check selected property for collection is null or not .Any().
    /// </summary>
    /// <typeparam name="T">The type of entities to validate</typeparam>
    /// <typeparam name="TEnumerable">The type argument of the enumerable collection</typeparam>
    /// <param name="ruleBuilder">The rule builder</param>
    /// <param name="selector">Property selector</param>
    public static IRuleBuilder<T> InvalidWhenNullOrEmpty<T, TEnumerable>(this IRuleBuilder<T> ruleBuilder, Func<T, IEnumerable<TEnumerable>> selector)
    {
        ruleBuilder.InvalidWhen(x =>
        {
            var collection = selector(x);

            return collection?.Any() != true;
        });

        return ruleBuilder;
    }

    /// <summary>
    /// Create invalid condition to check selected property for null.
    /// </summary>
    /// <typeparam name="T">The type of entities to validate</typeparam>
    /// <typeparam name="TProperty">The type of property to check</typeparam>
    /// <param name="ruleBuilder">The rule builder</param>
    /// <param name="selector">Property selector</param>
    public static IRuleBuilder<T> InvalidWhenNull<T, TProperty>(this IRuleBuilder<T> ruleBuilder, Func<T, TProperty> selector)
    {
        ruleBuilder.InvalidWhen(x =>
        {
            var obj = selector(x);

            return obj is null;
        });

        return ruleBuilder;
    }
}
