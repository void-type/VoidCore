namespace VoidCore.Model.Domain
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
        string Message { get; }

        /// <summary>
        /// The name of the UI field corresponding to the invalid user input.
        /// </summary>
        /// <value>The name of the field</value>
        string UiHandle { get; }
    }
}
