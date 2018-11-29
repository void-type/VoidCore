namespace VoidCore.Domain.Events
{
    /// <summary>
    /// A validator to be run against a validatable entity.
    /// </summary>
    /// <typeparam name="TValidatableEntity">The entity type</typeparam>
    public interface IRequestValidator<in TValidatableEntity>
    {
        /// <summary>
        /// Run the validator against the supplied entity and return validation errors if input is invalid.
        /// </summary>
        /// <param name="validatableEntity">The entity to validate</param>
        /// <returns>Validation errors or an empty array if input is valid.</returns>
        IResult Validate(TValidatableEntity validatableEntity);
    }
}
