namespace VoidCore.Model.Responses.Messages
{
    /// <summary>
    /// Static helpers to create UserMessages
    /// </summary>
    public partial class UserMessage
    {
        /// <summary>
        /// Create a new UserMessage.
        /// </summary>
        /// <param name="message">A UI-friendly message</param>
        public static UserMessage Create(string message)
        {
            return new UserMessage(message);
        }

        /// <summary>
        /// Create a new UserMessage with an entity Id
        /// </summary>
        /// <param name="id">The Id of the entity altered during an event</param>
        /// <param name="message">A UI-friendly message</param>
        /// <typeparam name="TId">The type of the Id</typeparam>
        public static UserMessage<TId> Create<TId>(string message, TId id)
        {
            return new UserMessage<TId>(message, id);
        }
    }
}
