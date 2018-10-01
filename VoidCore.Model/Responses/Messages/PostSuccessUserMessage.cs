namespace VoidCore.Model.Responses.Message
{
    /// <summary>
    /// Store a UI-friendly success message and the Id of the entity that was altered during the command.
    /// </summary>
    public class PostSuccessUserMessage<TId> : UserMessage
    {
        /// <summary>
        /// Create a new post success message.
        /// </summary>
        /// <param name="message">The Ui-Friendly message</param>
        /// <param name="id">The Id of the entity changed during the POST request</param>
        /// <returns></returns>
        internal PostSuccessUserMessage(string message, TId id) : base(message)
        {
            Id = id;
        }

        /// <summary>
        /// The Id of the entity changed during a POST request.
        /// </summary>
        /// <value></value>
        public TId Id { get; }
    }

    /// <summary>
    /// Static helpers to create PostSuccessUserMessages
    /// </summary>
    public class PostSuccessUserMessage
    {
        /// <summary>
        /// Create a new PostSuccessUserMessage
        /// </summary>
        /// <param name="id">The id of the entity affected in the command</param>
        /// <param name="message">A UI-friendly success message</param>
        /// <typeparam name="TId">The type of the Id</typeparam>
        /// <returns></returns>
        public static PostSuccessUserMessage<TId> Create<TId>(string message, TId id)
        {
            return new PostSuccessUserMessage<TId>(message, id);
        }
    }
}
