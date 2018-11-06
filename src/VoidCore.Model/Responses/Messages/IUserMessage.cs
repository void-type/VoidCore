namespace VoidCore.Model.Responses.Messages
{
    /// <summary>
    /// An interface for sending UI messages to the client.
    /// </summary>
    public interface IUserMessage
    {
        /// <summary>
        /// The UI-friendly message to send to the client.
        /// </summary>
        string Message { get; }
    }
}
