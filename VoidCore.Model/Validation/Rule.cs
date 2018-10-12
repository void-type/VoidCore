using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.DomainEvents;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// A rule for validating an entity.
    /// </summary>
    public class Rule<TValidatableEntity> : IRule<TValidatableEntity>, IRuleBuilder<TValidatableEntity>
    {
        /// <summary>
        /// Construct a new rule and underlying validation error to throw when violations are detected.
        /// </summary>
        /// <param name="failureBuilder">A function that builds a custom IFailure to return if the rule fails.</param>
        internal Rule(Func<TValidatableEntity, IFailure> failureBuilder)
        {
            _failureBuilder = failureBuilder;
        }

        /// <inheritdoc/>
        public IResult Run(TValidatableEntity validatableEntity)
        {
            if (!IsSuppressed(validatableEntity) && IsInvalid(validatableEntity))
            {
                return Result.Fail(_failureBuilder(validatableEntity));
            }

            return Result.Ok();
        }

        /// <inheritdoc/>
        public IRuleBuilder<TValidatableEntity> InvalidWhen(Func<TValidatableEntity, bool> invalidCondition)
        {
            _invalidConditions.Add(invalidCondition);
            return this;
        }

        /// <inheritdoc/>
        public IRuleBuilder<TValidatableEntity> ExceptWhen(Func<TValidatableEntity, bool> suppressCondition)
        {
            _suppressConditions.Add(suppressCondition);
            return this;
        }

        private bool IsSuppressed(TValidatableEntity validatableEntity)
        {
            return _suppressConditions.Any() && _suppressConditions.All(check => check(validatableEntity));
        }

        private bool IsInvalid(TValidatableEntity validatableEntity)
        {
            return _invalidConditions.Any() && _invalidConditions.Any(check => check(validatableEntity));
        }

        private readonly Func<TValidatableEntity, IFailure> _failureBuilder;
        private readonly List<Func<TValidatableEntity, bool>> _invalidConditions = new List<Func<TValidatableEntity, bool>>();
        private readonly List<Func<TValidatableEntity, bool>> _suppressConditions = new List<Func<TValidatableEntity, bool>>();
    }
}
