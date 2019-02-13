using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain.Events;

namespace VoidCore.Domain.RuleValidator
{
    /// <summary>
    /// The base for a custom rule-based entity validator.
    /// </summary>
    /// <typeparam name="T">The type of entities to validate</typeparam>
    public abstract class RuleValidatorAbstract<T> : IRequestValidator<T>
    {
        /// <inheritdoc/>
        public IResult Validate(T validatable)
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
        protected IRuleBuilder<T> CreateRule(string errorMessage, string uiHandle)
        {
            var rule = new Rule<T>(validatable => new Failure(errorMessage, uiHandle));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessageBuilder">The message to display to the user</param>
        /// <param name="uiHandleBuilder">The UI field name to tie the error to</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<T> CreateRule(Func<T, string> errorMessageBuilder, Func<T, string> uiHandleBuilder)
        {
            var rule = new Rule<T>(validatable => new Failure(errorMessageBuilder(validatable), uiHandleBuilder(validatable)));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessageBuilder">The message to display to the user</param>
        /// <param name="uiHandle">The UI field name to tie the error to</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<T> CreateRule(Func<T, string> errorMessageBuilder, string uiHandle)
        {
            var rule = new Rule<T>(validatable => new Failure(errorMessageBuilder(validatable), uiHandle));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessage">The message to display to the user</param>
        /// <param name="uiHandleBuilder">The UI field name to tie the error to</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<T> CreateRule(string errorMessage, Func<T, string> uiHandleBuilder)
        {
            var rule = new Rule<T>(validatable => new Failure(errorMessage, uiHandleBuilder(validatable)));
            _rules.Add(rule);
            return rule;
        }

        private readonly List<IRule<T>> _rules = new List<IRule<T>>();
    }
}
