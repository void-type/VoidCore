using System;
using VoidCore.Model.Action.Responses.File;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Validation;

namespace VoidCore.Model.Action.Responder
{
    /// <summary>
    /// Adapter for ActionSteps to use any response implementation.
    /// </summary>
    public abstract class AbstractActionResponder<TResponse> : IActionResponder where TResponse : class
    {
        /// <inheritdoc/>
        public bool IsResponseCreated => Response != default(TResponse);

        /// <summary>
        /// The concrete response to be returned. Null until a response is requested of the responder by an action. This is not visible within the action.
        /// </summary>
        /// <value></value>
        public TResponse Response { get; protected set; }

        /// <inheritdoc/>
        public abstract void WithError(string errorMessage, Exception exception = null, params string[] logMessages);

        /// <inheritdoc/>
        public abstract void WithError(ErrorUserMessage errorMessage, Exception exception = null, params string[] logMessages);

        /// <inheritdoc/>
        public abstract void WithSuccess(object resultItem, params string[] logMessages);

        /// <inheritdoc/>
        public abstract void WithSuccess(ISimpleFile file, params string[] logMessages);

        /// <inheritdoc/>
        public abstract void WithWarning(IItemSet<IFailure> validationErrors, params string[] logMessages);

        /// <inheritdoc/>
        public abstract void WithWarning(string warningMessage, params string[] logMessages);
    }
}
