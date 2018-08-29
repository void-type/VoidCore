using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.ItemSet;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Response with a collection of items.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RespondWithItemSet<TEntity> : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="set">The set to send to respond with</param>
        /// <param name="logMessage">Additonal logging information</param>
        public RespondWithItemSet(IEnumerable<TEntity> set, string logMessage = null)
        {
            _set = set;
            _logMessage = logMessage;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            var itemSet = _set.ToItemSet();
            var fullLogText = itemSet.GetLogText().Concat(new [] { _logMessage }).ToArray();
            respond.WithSuccess(itemSet, fullLogText);
        }

        private readonly string _logMessage;
        private readonly IEnumerable<TEntity> _set;
    }
}
