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
        public bool IsViolated { get; private set; }

        /// <inheritdoc/>
        public IFailure ValidationError { get; }

        /// <summary>
        /// Construct a new rule and underlying validation error to throw when violations are detected.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="fieldName"></param>
        public Rule(string errorMessage, string fieldName)
        {
            ValidationError = new Failure(errorMessage, fieldName);
        }

        /// <inheritdoc/>
        public IRuleBuilder ExceptWhen(bool suppressionCondition)
        {
            IsSuppressed = IsSuppressed || suppressionCondition;
            return this;
        }

        /// <inheritdoc/>
        public IRuleBuilder ValidWhen(bool validCondition)
        {
            IsViolated = IsViolated || !validCondition;
            return this;
        }
    }
}
