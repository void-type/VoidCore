using VoidCore.Model.Functional;

namespace VoidCore.Model.Events
{
    /// <summary>
    /// A validator to be run against a validatable entity.
    /// </summary>
    /// <typeparam name="T">The type of request to be validated</typeparam>
    public interface IRequestValidator<in T>
    {
        /// <summary>
        /// Run the validator against the supplied entity and return validation errors if input is invalid.
        /// </summary>
        /// <param name="request">The entity to validate</param>
        /// <returns>Validation errors or an empty array if input is valid.</returns>
        IResult Validate(T request);
    }
}
