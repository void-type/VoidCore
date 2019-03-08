using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidCore.Domain.RuleValidator
{
    /// <summary>
    /// A rule for validating an entity.
    /// </summary>
    public class Rule<T> : IRule<T>, IRuleBuilder<T>
    {
        private readonly Func<T, IFailure> _failureBuilder;
        private readonly List<Func<T, bool>> _invalidConditions = new List<Func<T, bool>>();
        private readonly List<Func<T, bool>> _suppressConditions = new List<Func<T, bool>>();

        /// <summary>
        /// Construct a new rule and underlying validation error to throw when violations are detected.
        /// </summary>
        /// <param name="failureBuilder">A function that builds a custom IFailure to return if the rule fails.</param>
        internal Rule(Func<T, IFailure> failureBuilder)
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

        /// <inheritdoc/>
        public IResult Run(T validatableEntity)
        {
            if (!IsSuppressed(validatableEntity) && IsInvalid(validatableEntity))
            {
                return Result.Fail(_failureBuilder(validatableEntity));
            }

            return Result.Ok();
        }

        private bool IsInvalid(T validatableEntity)
        {
            return _invalidConditions.Any() && _invalidConditions.Any(check => check(validatableEntity));
        }

        private bool IsSuppressed(T validatableEntity)
        {
            return _suppressConditions.Any() && _suppressConditions.Any(check => check(validatableEntity));
        }
    }
}
