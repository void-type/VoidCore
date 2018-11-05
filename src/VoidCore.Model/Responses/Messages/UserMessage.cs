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
        /// <param name="message"></param>
        public UserMessage(string message)
        {
            Message = message;
        }
    }
}
