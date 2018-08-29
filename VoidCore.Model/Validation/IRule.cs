namespace VoidCore.Model.Validation
{
    /// <summary>
    /// A validation rule to be run against a validatable entity.
    /// </summary>
    public interface IRule : IRuleBuilder
    {
        /// <summary>
        /// When true, the validator will not return this error regardless if it is violated.
        /// </summary>
        bool IsSuppressed { get; }

        /// <summary>
        /// When true, the rule has been violated and the error will be thrown.
        /// </summary>
        /// <value></value>
        bool IsViolated { get; }

        /// <summary>
        /// The validation error to be thrown if the rule is invalid and not suppressed.
        /// </summary>
        /// <value></value>
        IValidationError ValidationError { get; }
    }
}
