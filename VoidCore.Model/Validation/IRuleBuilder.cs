using System;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// Interface for building new validator rules.
    /// </summary>
    /// <typeparam name="TValidatableEntity"></typeparam>
    public interface IRuleBuilder<out TValidatableEntity>
    {
        /// <summary>
        /// Provide a function that returns true when the entity is invalid. All invalid conditions
        /// must be true for the entity to be invalid.
        /// </summary>
        /// <param name="invalidCondition">A function that returns true if the entity is invalid.</param>
        /// <returns>A rule builder to chain rule creation operations.</returns>
        IRuleBuilder<TValidatableEntity> InvalidWhen(Func<TValidatableEntity, bool> invalidCondition);

        /// <summary>
        /// Provide a function that returns true when a rule should be suppressed. All suppression conditions
        /// must be true for the rule to be suppressed.
        /// </summary>
        /// <param name="suppressCondition"></param>
        /// <returns>A rule builder to chain rule creation operations.</returns>
        IRuleBuilder<TValidatableEntity> ExceptWhen(Func<TValidatableEntity, bool> suppressCondition);
    }
}
