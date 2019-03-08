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
        private readonly List<IRule<T>> _rules = new List<IRule<T>>();

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
        /// <param name="failureBuilder">A builder function for the failure to give upon invalid entity.</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<T> CreateRule(Func<T, IFailure> failureBuilder)
        {
            var rule = new Rule<T>(validatable => failureBuilder(validatable));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="failure">The failure to give upon invalid entity.</param>
        /// <returns>A rule builder to continue building this rule</returns>
        protected IRuleBuilder<T> CreateRule(IFailure failure)
        {
            var rule = new Rule<T>(validatable => failure);
            _rules.Add(rule);
            return rule;
        }
    }
}
