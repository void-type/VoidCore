namespace VoidCore.Model.Responses.Messages
{
    /// <summary>
    /// A UI-friendly message and the Id of the entity that was altered during an event.
    /// </summary>
    public class UserMessageWithEntityId<TId> : UserMessage
    {
        /// <summary>
        /// Create a new UserMessageWithEntityId.
        /// </summary>
        /// <param name="message">The Ui-Friendly message</param>
        /// <param name="id">The Id of the entity altered during an event</param>
        internal UserMessageWithEntityId(string message, TId id) : base(message)
        {
            Id = id;
        }

        /// <summary>
        /// The Id of the entity altered during an event.
        /// </summary>
        public TId Id { get; }
    }

    /// <summary>
    /// Static helpers to create UserMessageWithEntityIds
    /// </summary>
    public static class UserMessageWithEntityId
    {
        /// <summary>
        /// Create a new UserMessageWithEntityId
        /// </summary>
        /// <param name="id">The Id of the entity altered during an event</param>
        /// <param name="message">A UI-friendly message</param>
        /// <typeparam name="TId">The type of the Id</typeparam>
        public static UserMessageWithEntityId<TId> Create<TId>(string message, TId id)
        {
            return new UserMessageWithEntityId<TId>(message, id);
        }
    }
}
