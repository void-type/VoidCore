namespace VoidCore.Model.Events
{
    /// <summary>
    /// A custom function to log the properties of the request or response of an event.
    /// </summary>
    /// <typeparam name="T">The type of object to log.</typeparam>
    public interface IRequestLogger<in T>
    {
        /// <summary>
        /// Implement this method to log the request or response object.
        /// </summary>
        /// <param name="request">The request or response</param>
        void Log(T request);
    }
}
