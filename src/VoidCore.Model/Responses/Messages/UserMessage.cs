namespace VoidCore.Model.Responses.Messages
{
    /// <summary>
    /// A UI-friendly message.
    /// </summary>
    public class UserMessage : IUserMessage
    {
        /// <inheritdoc/>
        public string Message { get; }

        /// <summary>
        /// Create a new message.
        /// </summary>
        /// <param name="message">The user message to display</param>
        public UserMessage(string message)
        {
            Message = message;
        }
    }
}
