using VoidCore.Model.DomainEvents;

namespace VoidCore.Model.Validation
{
    /// <summary>
    /// A validator to be run against a validatable entity.
    /// </summary>
    /// <typeparam name="TValidatableEntity">The entity type</typeparam>
    public interface IValidator<in TValidatableEntity>
    {
        /// <summary>
        /// Run the validator against the supplied entity and return validation errors if input is invalid.
        /// </summary>
        /// <param name="validatableEntity"></param>
        /// <returns>Validation errors or an empty array if input is valid.</returns>
        IResult Validate(TValidatableEntity validatableEntity);
    }
}
