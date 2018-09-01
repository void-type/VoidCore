namespace VoidCore.Model.Action.Railway
{
    /// <inheritdoc/>
    public class Failure : IFailure
    {
        /// <inheritdoc/>
        public string ErrorMessage { get; }

        /// <inheritdoc/>
        public string UiHandle { get; }

        /// <summary>
        /// Construct a new Validation Error.
        /// </summary>
        /// <param name="errorMessage">UI friendly error message</param>
        /// <param name="uiHandle">The entity property name that is in error. Can be mapped to a field on the view</param>
        public Failure(string errorMessage = null, string uiHandle = null)
        {
            ErrorMessage = errorMessage;
            UiHandle = uiHandle;
        }
    }
}
