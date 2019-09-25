namespace VoidCore.Model.Responses.Messages
{
    /// <summary>
    /// Static helpers to create messages
    /// </summary>
    public static class EntityMessage
    {
        /// <summary>
        /// Create a new UserMessage with an entity Id
        /// </summary>
        /// <param name="id">The Id of the entity affected during an event</param>
        /// <param name="message">A UI-friendly message</param>
        /// <typeparam name="TId">The type of the Id</typeparam>
        public static EntityMessage<TId> Create<TId>(string message, TId id)
        {
            return new EntityMessage<TId>(message, id);
        }
    }
}
