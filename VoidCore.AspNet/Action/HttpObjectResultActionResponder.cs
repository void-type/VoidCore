using Microsoft.AspNetCore.Mvc;
using System;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.File;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Logging;
using VoidCore.Model.Validation;

namespace VoidCore.AspNet.Action
{
    /// <summary>
    /// An action chain responder that returns AspNet IActionResults.
    /// </summary>
    public class HttpObjectResultActionResponder : AbstractActionResponder<IActionResult>
    {
        /// <summary>
        /// Construct a new responder.
        /// </summary>
        /// <param name="logger"></param>
        public HttpObjectResultActionResponder(ILoggingService logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public override void WithError(ErrorUserMessage errorMessage, Exception exception = null, params string[] logMessages)
        {
            _logger.Error(exception, logMessages);
            Response = new ObjectResult(errorMessage) { StatusCode = 500 };
        }

        /// <inheritdoc/>
        public override void WithError(string errorMessage, Exception exception = null, params string[] logMessages)
        {
            var errorUserMessage = new ErrorUserMessage(errorMessage);
            WithError(errorUserMessage, exception, logMessages);
        }

        /// <inheritdoc/>
        public override void WithSuccess(object resultItem, params string[] logMessages)
        {
            _logger.Info(logMessages);
            Response = new ObjectResult(resultItem) { StatusCode = 200 };
        }

        /// <inheritdoc/>
        public override void WithSuccess(ISimpleFile file, params string[] logMessages)
        {
            var fileResponse =
                new FileContentResult(file.Content, "application/force-download") { FileDownloadName = file.Name };
            _logger.Info(logMessages);
            Response = fileResponse;
        }

        /// <inheritdoc/>
        public override void WithWarning(IItemSet<IFailure> validationErrors, params string[] logMessages)
        {
            _logger.Warn(logMessages);
            Response = new ObjectResult(validationErrors) { StatusCode = 400 };
        }

        /// <inheritdoc/>
        public override void WithWarning(string warningMessage, params string[] logMessages)
        {
            var validationErrors = new IFailure[] { new Failure(warningMessage) }.ToItemSet();
            WithWarning(validationErrors, logMessages);
        }

        private readonly ILoggingService _logger;
    }
}
