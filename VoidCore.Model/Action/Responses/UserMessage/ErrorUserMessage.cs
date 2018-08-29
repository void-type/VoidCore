namespace VoidCore.Model.Action.Responses.UserMessage
{
    /// <summary>
    /// Stores a UI-friendly error message for critical or non-user caused errors.
    /// </summary>
    public class ErrorUserMessage : AbstractUserMessage
    {
        /// <inheritdoc/>
        public ErrorUserMessage(string message) : base(message)
        { }
    }
}
