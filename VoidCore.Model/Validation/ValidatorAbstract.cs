using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.DomainEvents;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// The base for a custom entity validator.
    /// </summary>
    /// <typeparam name="TValidatableEntity">The type of entities to validate</typeparam>
    public abstract class ValidatorAbstract<TValidatableEntity> : IValidator<TValidatableEntity>
    {
        /// <inheritdoc/>
        public IResult Validate(TValidatableEntity validatable)
        {
            return _rules
                .Select(rule => rule.Validate(validatable))
                .Combine();
        }

        /// <summary>
        /// Construct a new validator.
        /// </summary>
        protected ValidatorAbstract()
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
            var rule = new Rule<TValidatableEntity>(new Func<TValidatableEntity, IFailure>(validatable => new Failure(errorMessage, uiHandle)));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessageBuilder">The message to display to the user</param>
        /// <param name="uiHandleBuilder">The UI field name to tie the error to</param>
        /// <returns></returns>
        protected IRuleBuilder<TValidatableEntity> CreateRule(Func<TValidatableEntity, string> errorMessageBuilder, Func<TValidatableEntity, string> uiHandleBuilder)
        {
            var rule = new Rule<TValidatableEntity>(new Func<TValidatableEntity, IFailure>(validatable => new Failure(errorMessageBuilder.Invoke(validatable), uiHandleBuilder.Invoke(validatable))));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessageBuilder">The message to display to the user</param>
        /// <param name="uiHandle">The UI field name to tie the error to</param>
        /// <returns></returns>
        protected IRuleBuilder<TValidatableEntity> CreateRule(Func<TValidatableEntity, string> errorMessageBuilder, string uiHandle)
        {
            var rule = new Rule<TValidatableEntity>(new Func<TValidatableEntity, IFailure>(validatable => new Failure(errorMessageBuilder.Invoke(validatable), uiHandle)));
            _rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Create a new rule for this entity.
        /// </summary>
        /// <param name="errorMessage">The message to display to the user</param>
        /// <param name="uiHandleBuilder">The UI field name to tie the error to</param>
        /// <returns></returns>
        protected IRuleBuilder<TValidatableEntity> CreateRule(string errorMessage, Func<TValidatableEntity, string> uiHandleBuilder)
        {
            var rule = new Rule<TValidatableEntity>(new Func<TValidatableEntity, IFailure>(validatable => new Failure(errorMessage, uiHandleBuilder.Invoke(validatable))));
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
