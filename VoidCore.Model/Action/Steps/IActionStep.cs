using VoidCore.Model.Action.Responder;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// A single step in an action chain. Encapsulates a model action.
    /// </summary>
    public interface IActionStep
    {
        /// <summary>
        /// Invoke the action step.
        /// </summary>
        /// <param name="respond">The ActionResponder that handles output of the action chain</param>
        void Perform(IActionResponder respond);
    }
}
