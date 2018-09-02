namespace VoidCore.Model.Action.Responses.Message
{
    /// <summary>
    /// Store a UI-friendly success message and the Id of the entity that was altered.
    /// </summary>
    public class PostSuccessUserMessage<TId> : UserMessage
    {
        /// <summary>
        /// Create a new post success message.
        /// </summary>
        /// <param name="message">The Ui-Friendly message</param>
        /// <param name="id">The Id of the entity changed during the POST request</param>
        /// <returns></returns>
        public PostSuccessUserMessage(string message, TId id) : base(message)
        {
            Id = id;
        }

        /// <summary>
        /// The Id of the entity changed during a POST request.
        /// </summary>
        /// <value></value>
        public TId Id { get; }
    }
}
