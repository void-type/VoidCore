using System;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Steps;

namespace VoidCore.Model.Action.Chain
{
    /// <summary>
    /// ActionChain injects the responder into each action step and stops execution when a response is set.
    /// It also wraps domain logic in an exception logger for uncaught exceptions.
    /// </summary>
    public class ActionChain : IActionChain
    {
        /// <summary>
        /// Construct a new action chain.
        /// </summary>
        /// <param name="responder">The responder for the chain</param>
        public ActionChain(IActionResponder responder)
        {
            _respond = responder;
        }

        /// <inheritdoc/>
        public IActionChain Execute(IActionStep step)
        {
            if (_respond.IsResponseCreated)
            {
                return this;
            }
            try
            {
                step.Perform(_respond);
            }
            catch (Exception exception)
            {
                _respond.WithError("There was a problem processing your request.", exception, $"StepName: {step.GetType()}");
            }
            return this;
        }

        private readonly IActionResponder _respond;
    }
}
