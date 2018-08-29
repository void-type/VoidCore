namespace VoidCore.Model.Action.Responses.UserMessage
{
    /// <summary>
    /// Stores a UI-friendly success message.
    /// </summary>
    public class SuccessUserMessage : AbstractUserMessage
    {
        /// <inheritdoc/>
        public SuccessUserMessage(string message) : base(message) { }
    }
}
