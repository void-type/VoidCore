namespace VoidCore.Model.Action.Responses.UserMessage
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
        public static string[] GetLogText(this SuccessUserMessage successMessage)
        {
            return new []
            {
                $"SuccessMessage: {successMessage.Message}"
            };
        }

        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static string[] GetLogText(this ErrorUserMessage errorMessage)
        {
            return new []
            {
                $"ErrorMessage: {errorMessage.Message}"
            };
        }

        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="postSuccessMessage"></param>
        /// <returns></returns>
        public static string[] GetLogText(this PostSuccessUserMessage postSuccessMessage)
        {
            return new []
            {
                $"SuccessMessage: {postSuccessMessage.Message}",
                $"EntityId: {postSuccessMessage.Id}"
            };
        }
    }
}
