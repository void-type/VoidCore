using VoidCore.Model.Domain;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// A validation rule to be run against a validatable entity.
    /// </summary>
    public interface IRule<in TValidatableEntity>
    {
        /// <summary>
        /// Validate the entity and return a result.
        /// </summary>
        /// <param name="validatableEntity">The entity to validate</param>
        /// <returns></returns>
        IResult Run(TValidatableEntity validatableEntity);
    }
}
