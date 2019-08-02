namespace VoidCore.Model.Responses.Messages
{
    /// <summary>
    /// A UI-friendly message and the Id of the entity that was altered during an event.
    /// </summary>
    public class UserMessage<TId> : UserMessage
    {
        /// <summary>
        /// Create a new UserMessage with an entity Id.
        /// </summary>
        /// <param name="message">The Ui-Friendly message</param>
        /// <param name="id">The Id of the entity altered during an event</param>
        internal UserMessage(string message, TId id) : base(message)
        {
            Id = id;
        }

        /// <summary>
        /// The Id of the entity altered during an event.
        /// </summary>
        public TId Id { get; }
    }
}
