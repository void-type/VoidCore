namespace VoidCore.Domain.RuleValidator
{
    /// <summary>
    /// A validation rule to be run against a validatable entity.
    /// </summary>
    public interface IRule<in T>
    {
        /// <summary>
        /// Validate the entity and return a result.
        /// </summary>
        /// <param name="validatableEntity">The entity to validate</param>
        /// <returns>The result of running the rule against the entity</returns>
        IResult Run(T validatableEntity);
    }
}
