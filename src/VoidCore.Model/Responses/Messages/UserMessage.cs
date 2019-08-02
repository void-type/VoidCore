namespace VoidCore.Model.Responses.Messages
{
    /// <summary>
    /// A UI-friendly message.
    /// </summary>
    public partial class UserMessage
    {
        /// <summary>
        /// Create a new message.
        /// </summary>
        /// <param name="message">The user message to display</param>
        internal UserMessage(string message)
        {
            Message = message;
        }

        /// <summary>
        /// The UI-friendly message.
        /// </summary>
        public string Message { get; }
    }
}
