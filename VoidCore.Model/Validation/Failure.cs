namespace VoidCore.Model.Validation
{
    /// <inheritdoc/>
    public class Failure : IFailure
    {
        /// <inheritdoc/>
        public string ErrorMessage { get; set; }

        /// <inheritdoc/>
        public string UiHandle { get; set; }

        /// <summary>
        /// Construct a new Validation Error.
        /// </summary>
        /// <param name="errorMessage">UI friendly error message</param>
        /// <param name="fieldName">The entity property name that is in error. Can be mapped to a field on the view</param>
        public Failure(string errorMessage = null, string fieldName = null)
        {
            ErrorMessage = errorMessage;
            UiHandle = fieldName;
        }
    }
}
