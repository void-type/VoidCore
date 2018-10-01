namespace VoidCore.Model.Responses.Message
{
    /// <summary>
    /// An interface for sending UI messages to the client.
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
