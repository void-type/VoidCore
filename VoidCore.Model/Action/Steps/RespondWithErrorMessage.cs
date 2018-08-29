using System;
using System.Linq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.UserMessage;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Respond with an error message.
    /// </summary>
    public class RespondWithErrorMessage : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="message">An error message to show the user</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="logMessage">Additonal logging information</param>
        public RespondWithErrorMessage(string message, Exception exception, string logMessage = null)
        {
            _message = message;
            _logMessage = logMessage;
            _exception = exception;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            var error = new ErrorUserMessage(_message);
            var fullLogText = error.GetLogText().Concat(new [] { _logMessage }).ToArray();
            respond.WithError(error, _exception, fullLogText);
        }

        private readonly string _logMessage;
        private readonly Exception _exception;
        private readonly string _message;
    }
}
