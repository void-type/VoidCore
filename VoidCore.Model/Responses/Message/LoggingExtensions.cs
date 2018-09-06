namespace VoidCore.Model.Responses.Message
{
    /// <summary>
    /// Helpers for pulling logging text from the default responses.
    /// </summary>
    public static class ResponseLoggingExtensions
    {
        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        public static string[] GetLogText(this UserMessage successMessage)
        {
            return new []
            {
                $"Message: {successMessage.Message}"
            };
        }

        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="postSuccessMessage"></param>
        /// <returns></returns>
        public static string[] GetLogText<TId>(this PostSuccessUserMessage<TId> postSuccessMessage)
        {
            return new []
            {
                $"Message: {postSuccessMessage.Message}",
                $"EntityId: {postSuccessMessage.Id.ToString()}"
            };
        }
    }
}
