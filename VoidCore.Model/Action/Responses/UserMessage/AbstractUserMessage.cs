namespace VoidCore.Model.Action.Responses.UserMessage
{
    /// <summary>
    /// An abstract implmentation of the message interface.
    /// </summary>
    public class AbstractUserMessage : IUserMessage
    {
        /// <inheritdoc/>
        public string Message { get; }

        /// <summary>
        /// Create a new message.
        /// </summary>
        /// <param name="message"></param>
        protected AbstractUserMessage(string message)
        {
            Message = message;
        }
    }
}
