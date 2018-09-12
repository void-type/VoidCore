using VoidCore.Model.DomainEvents;

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
        /// <param name="validatable"></param>
        /// <returns></returns>
        IResult Validate(TValidatableEntity validatable);
    }
}
