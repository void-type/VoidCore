using System.Linq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.File;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Response with a collection of items.
    /// </summary>
    public class RespondWithFile : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="file">The set to send to respond with</param>
        /// <param name="logMessage">Additonal logging information</param>
        public RespondWithFile(ISimpleFile file, string logMessage = null)
        {
            _file = file;
            _logMessage = logMessage;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            var fullLogText = _file.GetLogText().Concat(new [] { _logMessage }).ToArray();
            respond.WithSuccess(_file, fullLogText);
        }

        private readonly ISimpleFile _file;
        private readonly string _logMessage;
    }
}
