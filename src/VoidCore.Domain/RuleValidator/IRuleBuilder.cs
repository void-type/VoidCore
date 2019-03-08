using System;

namespace VoidCore.Domain.RuleValidator
{
    /// <summary>
    /// Interface for building new validator rules.
    /// </summary>
    /// <typeparam name="T">The entity type to validate</typeparam>
    public interface IRuleBuilder<out T>
    {
        /// <summary>
        /// Provide a function that returns true when the entity is invalid. Any conditions that are false will
        /// invalidate the entity.
        /// </summary>
        /// <param name="invalidCondition">A function that returns true if the entity is invalid.</param>
        /// <returns>A rule builder to chain rule creation operations.</returns>
        IRuleBuilder<T> InvalidWhen(Func<T, bool> invalidCondition);

        /// <summary>
        /// Provide a function that returns true when a rule should be suppressed. Any conditions that are true can
        /// suppress the invalid message.
        /// </summary>
        /// <param name="suppressCondition">A function that returns true if the rule should be suppressed.</param>
        /// <returns>A rule builder to chain rule creation operations.</returns>
        IRuleBuilder<T> ExceptWhen(Func<T, bool> suppressCondition);
    }
}
