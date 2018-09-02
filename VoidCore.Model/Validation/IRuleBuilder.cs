using System;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// Interface for building new validator rules.
    /// </summary>
    /// <typeparam name="TValidatableEntity"></typeparam>
    public interface IRuleBuilder<TValidatableEntity>
    {
        /// <summary>
        /// Provide a function that returns true when the entity is valid. All valid conditions
        /// must be true for the entity to be validated.
        /// </summary>
        /// <param name="validCondition">A function to validate a part of the entity.</param>
        /// <returns>A rule builder to chain rule creation operations.</returns>
        IRuleBuilder<TValidatableEntity> ValidWhen(Func<TValidatableEntity, bool> validCondition);

        /// <summary>
        /// Provide a function that returns true when a rule should be suppressed. All suppression conditions
        /// must be true for the rule to be suppressed.
        /// </summary>
        /// <param name="suppressCondition"></param>
        /// <returns>A rule builder to chain rule creation operations.</returns>
        IRuleBuilder<TValidatableEntity> ExceptWhen(Func<TValidatableEntity, bool> suppressCondition);
    }
}
