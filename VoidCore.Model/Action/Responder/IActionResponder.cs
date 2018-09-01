using System;
using VoidCore.Model.Action.Responses.File;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Action.Responder
{
    /// <summary>
    /// ActionResponder retains validation errors between steps and sets the final response.
    /// </summary>
    public interface IActionResponder
    {
        /// <summary>
        /// Checks the response for a non-default value.
        /// </summary>
        bool IsResponseCreated { get; }

        /// <summary>
        /// Create a response with a fatal error message. The request was not completed successfully.
        /// </summary>
        /// <param name="errorMessage">The user-friendly error message</param>
        /// <param name="exception">An optional exception to log</param>
        /// <param name="logMessages">An array of strings to log</param>
        void WithError(string errorMessage, Exception exception = null, params string[] logMessages);

        /// <summary>
        /// Create a response with a fatal error message. The request was not completed successfully.
        /// </summary>
        /// <param name="errorMessage">The error message object</param>
        /// <param name="exception">An optional exception to log</param>
        /// <param name="logMessages">An array of strings to log</param>
        void WithError(ErrorUserMessage errorMessage, Exception exception = null, params string[] logMessages);

        /// <summary>
        /// Create a response with an object.
        /// </summary>
        /// <param name="resultItem">The action result</param>
        /// <param name="logMessages">An array of strings to log</param>
        void WithSuccess(object resultItem, params string[] logMessages);

        /// <summary>
        /// Create a response with a File result.
        /// </summary>
        /// <param name="file">A file to send to the client</param>
        /// <param name="logMessages">An array of strings to log</param>
        void WithSuccess(ISimpleFile file, params string[] logMessages);

        /// <summary>
        /// Warn the user of invalid actions or input. The request was not completed successfully.
        /// </summary>
        /// <param name="validationErrors">The validation errors</param>
        /// <param name="logMessages">An array of strings to log</param>
        void WithWarning(IItemSet<IFailure> validationErrors, params string[] logMessages);

        /// <summary>
        /// Warn the user of invalid actions or inputs. The request was not completed successfully.
        /// </summary>
        /// <param name="warningMessage">A warning message to display to the user</param>
        /// <param name="logMessages">An array of strings to log</param>
        void WithWarning(string warningMessage, params string[] logMessages);
    }
}
