using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Domain;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// The base for a custom rule-based entity validator.
    /// </summary>
    /// <typeparam name="TValidatableEntity">The type of entities to validate</typeparam>
    public abstract class RuleValidatorAbstract<TValidatableEntity> : IRequestValidator<TValidatableEntity>
    {
        /// <inheritdoc/>
        public IResult Validate(TValidatableEntity validatable)
        {
            return _rules
                .Select(rule => rule.Run(validatable))
                .Combine();
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessage">The message to display to the user</param>
        /// <param name="uiHandle">The UI field name to tie the error to</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<TValidatableEntity> CreateRule(string errorMessage, string uiHandle)
        {
            var rule = new Rule<TValidatableEntity>(validatable => new Failure(errorMessage, uiHandle));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessageBuilder">The message to display to the user</param>
        /// <param name="uiHandleBuilder">The UI field name to tie the error to</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<TValidatableEntity> CreateRule(Func<TValidatableEntity, string> errorMessageBuilder, Func<TValidatableEntity, string> uiHandleBuilder)
        {
            var rule = new Rule<TValidatableEntity>(validatable => new Failure(errorMessageBuilder(validatable), uiHandleBuilder(validatable)));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessageBuilder">The message to display to the user</param>
        /// <param name="uiHandle">The UI field name to tie the error to</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<TValidatableEntity> CreateRule(Func<TValidatableEntity, string> errorMessageBuilder, string uiHandle)
        {
            var rule = new Rule<TValidatableEntity>(validatable => new Failure(errorMessageBuilder(validatable), uiHandle));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessage">The message to display to the user</param>
        /// <param name="uiHandleBuilder">The UI field name to tie the error to</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<TValidatableEntity> CreateRule(string errorMessage, Func<TValidatableEntity, string> uiHandleBuilder)
        {
            var rule = new Rule<TValidatableEntity>(validatable => new Failure(errorMessage, uiHandleBuilder(validatable)));
            _rules.Add(rule);
            return rule;
        }

        private readonly List<IRule<TValidatableEntity>> _rules = new List<IRule<TValidatableEntity>>();
    }
}
