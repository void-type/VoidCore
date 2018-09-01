namespace VoidCore.Model.Validation
{
    /// <summary>
    /// A UI-friendly error message with optional field name.
    /// </summary>
    public interface IFailure
    {
        /// <summary>
        /// The UI-friendly error message to be displayed to the user.
        /// </summary>
        /// <value>The message</value>
        string ErrorMessage { get; set; }

        /// <summary>
        /// The name of the UI field corresponding to the invalid user input.
        /// </summary>
        /// <value>The name of the field</value>
        string UiHandle { get; set; }
    }
}
