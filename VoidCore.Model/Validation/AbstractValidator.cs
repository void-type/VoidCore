using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// The base for a custom entity validator.
    /// </summary>
    /// <typeparam name="TValidatableEntity">The type of entities to validate</typeparam>
    public abstract class AbstractValidator<TValidatableEntity> : IValidator<TValidatableEntity>
    {
        /// <inheritdoc/>
        public IEnumerable<IFailure> Validate(TValidatableEntity validatable)
        {
            _rules = new List<IRule>();

            BuildRules(validatable);

            return _rules
                .Where(rule => rule.IsViolated)
                .Where(rule => !rule.IsSuppressed)
                .Select(rule => rule.ValidationError);
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessage">The message to display to the user</param>
        /// <param name="fieldName">The UI field name to tie the error to</param>
        /// <returns></returns>
        protected IRuleBuilder CreateRule(string errorMessage, string fieldName)
        {
            var rule = new Rule(errorMessage, fieldName);
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Override this method to build the validation ruleset.
        /// Eg: Invalid("xValue","Must be true unless null").When(x.value != true).ExceptWhen(x.value == null);
        /// </summary>
        /// <param name="validatable">The entity to validate properties of</param>
        protected abstract void BuildRules(TValidatableEntity validatable);

        /// <summary>
        /// A temporary variable to hold rules while they are being validated.
        /// </summary>
        private List<IRule> _rules;
    }
}
