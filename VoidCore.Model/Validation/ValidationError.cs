namespace VoidCore.Model.Validation
{
    /// <inheritdoc/>
    public class ValidationError : IValidationError
    {
        /// <inheritdoc/>
        public string ErrorMessage { get; set; }

        /// <inheritdoc/>
        public string FieldName { get; set; }

        /// <summary>
        /// Construct a new Validation Error.
        /// </summary>
        /// <param name="errorMessage">UI friendly error message</param>
        /// <param name="fieldName">The entity property name that is in error. Can be mapped to a field on the view</param>
        public ValidationError(string errorMessage = null, string fieldName = null)
        {
            ErrorMessage = errorMessage;
            FieldName = fieldName;
        }
    }
}
