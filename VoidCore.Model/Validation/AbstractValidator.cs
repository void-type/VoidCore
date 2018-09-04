using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Railway;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// The base for a custom entity validator.
    /// </summary>
    /// <typeparam name="TValidatableEntity">The type of entities to validate</typeparam>
    public abstract class AbstractValidator<TValidatableEntity> : IValidator<TValidatableEntity>
    {
        /// <inheritdoc/>
        public Result Validate(TValidatableEntity validatable)
        {
            var ruleResults = _rules
                .Select(rule => rule.Validate(validatable))
                .ToArray();

            return Result.Combine(ruleResults);
        }

        /// <summary>
        /// Construct a new validator.
        /// </summary>
        protected AbstractValidator()
        {
            CallBuildRules();
        }

        /// <summary>
        /// Override this method to build the validation ruleset.
        /// Eg: Invalid("xValue","Must be true unless null").When(x.value != true).ExceptWhen(x.value == null);
        /// </summary>
        protected abstract void BuildRules();

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessage">The message to display to the user</param>
        /// <param name="uiHandle">The UI field name to tie the error to</param>
        /// <returns></returns>
        protected IRuleBuilder<TValidatableEntity> CreateRule(string errorMessage, string uiHandle)
        {
            var rule = Rule<TValidatableEntity>.Create(errorMessage, uiHandle);
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// A temporary variable to hold rules while they are being validated.
        /// </summary>
        private readonly List<IRule<TValidatableEntity>> _rules = new List<IRule<TValidatableEntity>>();

        private void CallBuildRules()
        {
            BuildRules();
        }
    }
}
