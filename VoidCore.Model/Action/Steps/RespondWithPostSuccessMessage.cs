using System.Linq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.UserMessage;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Respond with a success message.
    /// </summary>
    public class RespondWithPostSuccessMessage : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="message">A success message to show the user</param>
        /// <param name="id">The entity's persistence ID</param>
        /// <param name="logMessage">Additonal logging information</param>
        public RespondWithPostSuccessMessage(string message, string id, string logMessage = null)
        {
            _message = message;
            _id = id;
            _logMessage = logMessage;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            var postSuccess = new PostSuccessUserMessage(_message, _id);
            var fullLogText = postSuccess.GetLogText().Concat(new [] { _logMessage }).ToArray();
            respond.WithSuccess(postSuccess, fullLogText);
        }

        private readonly string _id;
        private readonly string _logMessage;
        private readonly string _message;
    }
}
