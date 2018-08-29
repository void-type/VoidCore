using VoidCore.Model.Action.Steps;

namespace VoidCore.Model.Action.Chain
{
    /// <summary>
    /// The interface for an chain of actions.
    /// </summary>
    public interface IActionChain
    {
        /// <summary>
        /// Execute the next action in the chain.
        /// </summary>
        /// <param name="step">The step to execute in the chain</param>
        /// <returns>The action chain</returns>
        IActionChain Execute(IActionStep step);
    }
}
