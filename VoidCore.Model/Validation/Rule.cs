namespace VoidCore.Model.Validation
{
    /// <summary>
    /// A rule for validating an entity.
    /// </summary>
    public class Rule : IRule
    {
        /// <inheritdoc/>
        public bool IsSuppressed { get; private set; }

        /// <inheritdoc/>
        public bool IsValid { get; private set; } = true;

        /// <inheritdoc/>
        public IValidationError ValidationError { get; }

        /// <summary>
        /// Construct a new rule and underlying validation error to throw when violations are detected.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="fieldName"></param>
        public Rule(string errorMessage, string fieldName)
        {
            ValidationError = new ValidationError(errorMessage, fieldName);
        }

        /// <inheritdoc/>
        public IRuleBuilder ExceptWhen(bool suppressionCondition)
        {
            IsSuppressed = !IsSuppressed && suppressionCondition;
            return this;
        }

        /// <inheritdoc/>
        public IRuleBuilder When(bool invalidCondition)
        {
            IsValid = IsValid && !invalidCondition;
            return this;
        }
    }
}
