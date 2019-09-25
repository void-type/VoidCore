namespace VoidCore.Model.Responses.Messages
{
    /// <summary>
    /// A UI-friendly message and the Id of the entity that was affected during an event.
    /// </summary>
    public class EntityMessage<TId> : UserMessage
    {
        /// <summary>
        /// Create a new UserMessage with an entity Id.
        /// </summary>
        /// <param name="message">The Ui-Friendly message</param>
        /// <param name="id">The Id of the entity affected during an event</param>
        internal EntityMessage(string message, TId id) : base(message)
        {
            Id = id;
        }

        /// <summary>
        /// The Id of the entity affected during an event.
        /// </summary>
        public TId Id { get; }
    }
}
