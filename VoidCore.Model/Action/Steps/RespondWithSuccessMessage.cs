using System.Linq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.UserMessage;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Respond with a success message.
    /// </summary>
    public class RespondWithSuccessMessage : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="message">A success message to show the user</param>
        /// <param name="logMessage">Additonal logging information</param>
        public RespondWithSuccessMessage(string message, string logMessage = null)
        {
            _message = message;
            _logMessage = logMessage;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            var success = new SuccessUserMessage(_message);
            var fullLogText = success.GetLogText().Concat(new [] { _logMessage }).ToArray();
            respond.WithSuccess(success, fullLogText);
        }

        private readonly string _logMessage;
        private readonly string _message;
    }
}
