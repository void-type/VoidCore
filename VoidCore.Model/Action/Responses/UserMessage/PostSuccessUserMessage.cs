namespace VoidCore.Model.Action.Responses.UserMessage
{
    /// <summary>
    /// Store a UI-friendly success message and the Id of the entity that was altered.
    /// </summary>
    public class PostSuccessUserMessage : SuccessUserMessage
    {
        /// <summary>
        /// Create a new post success message.
        /// </summary>
        /// <param name="message">The Ui-Friendly message</param>
        /// <param name="id">The Id of the entity changed during the POST request</param>
        /// <returns></returns>
        public PostSuccessUserMessage(string message, string id) : base(message)
        {
            Id = id;
        }

        /// <summary>
        /// Create a new post success message.
        /// </summary>
        /// <param name="message">The Ui-Friendly message</param>
        /// <param name="id">The Id of the entity changed during the POST request</param>
        /// <returns></returns>
        public PostSuccessUserMessage(string message, int id) : base(message)
        {
            Id = id.ToString();
        }

        /// <summary>
        /// The Id of the entity changed during a POST request.
        /// </summary>
        /// <value></value>
        public string Id { get; }
    }
}
