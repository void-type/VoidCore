namespace VoidCore.Model.Action.Responses.UserMessage
{
    /// <summary>
    /// An interface for sending messages to the client.
    /// </summary>
    public interface IUserMessage
    {
        /// <summary>
        /// The UI-friendly message to send to the client.
        /// </summary>
        /// <value></value>
        string Message { get; }
    }
}
