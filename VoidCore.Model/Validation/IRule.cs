namespace VoidCore.Model.Validation
{
    /// <summary>
    /// A validation rule to be run against a validatable entity.
    /// </summary>
    public interface IRule : IRuleBuilder
    {
        /// <summary>
        /// When true, the validator will not return this error.
        /// </summary>
        bool IsSuppressed { get; }

        /// <summary>
        /// When true, the rule has not been violated and no error will be thrown.
        /// </summary>
        /// <value></value>
        bool IsValid { get; }

        /// <summary>
        /// The validation error to be thrown if the rule is invalid and not suppressed.
        /// </summary>
        /// <value></value>
        IValidationError ValidationError { get; }
    }
}
