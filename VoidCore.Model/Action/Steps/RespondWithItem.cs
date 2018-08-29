using VoidCore.Model.Action.Responder;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Respond with an item.
    /// </summary>
    /// <typeparam name="TEntity">The type of item to respond with</typeparam>
    public class RespondWithItem<TEntity> : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="item">The item to respond with</param>
        /// <param name="logMessage">Additonal logging information</param>
        public RespondWithItem(TEntity item, string logMessage = null)
        {
            _item = item;
            _logMessage = logMessage;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            respond.WithSuccess(_item, _logMessage);
        }

        private readonly TEntity _item;
        private readonly string _logMessage;
    }
}
