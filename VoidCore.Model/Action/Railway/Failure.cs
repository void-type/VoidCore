namespace VoidCore.Model.Action.Railway
{
    /// <summary>
    /// A domain logic failure with UI-friendly error message and optional field name or UI handle.
    /// </summary>
    public interface IFailure
    {
        /// <summary>
        /// The UI-friendly error message to be displayed to the user.
        /// </summary>
        /// <value>The message</value>
        string ErrorMessage { get; }

        /// <summary>
        /// The name of the UI field corresponding to the invalid user input.
        /// </summary>
        /// <value>The name of the field</value>
        string UiHandle { get; }
    }
    
    /// <inheritdoc/>
    public class Failure : IFailure
    {
        /// <inheritdoc/>
        public string ErrorMessage { get; }

        /// <inheritdoc/>
        public string UiHandle { get; }

        /// <summary>
        /// Construct a new Failure.
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
