using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.ItemSet;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Respond with a page of a set of items.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RespondWithItemSetPage<TEntity> : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="set">The set to respond with</param>
        /// <param name="take">The requested number of items</param>
        /// <param name="page">The page number to get items from</param>
        /// <param name="logMessage">Additonal logging information</param>
        public RespondWithItemSetPage(IEnumerable<TEntity> set, int page, int take, string logMessage = null)
        {
            _set = set;
            _take = take;
            _page = page;
            _logMessage = logMessage;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            var itemSetPage = _set.ToItemSetPage(_page, _take);
            var fullLogText = itemSetPage.GetLogText().Concat(new [] { _logMessage }).ToArray();
            respond.WithSuccess(itemSetPage, fullLogText);
        }

        private readonly string _logMessage;
        private readonly int _page;
        private readonly IEnumerable<TEntity> _set;
        private readonly int _take;
    }
}
