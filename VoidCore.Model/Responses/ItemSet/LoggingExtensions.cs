using System.Linq;
using VoidCore.Model.Railway;

namespace VoidCore.Model.Responses.ItemSet
{
    /// <summary>
    /// Helpers for pulling logging text from the default responses.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="itemSet"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static string[] GetLogText<TEntity>(this IItemSet<TEntity> itemSet)
        {
            return new []
            {
                $"Count: {itemSet.Count}"
            };
        }

        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="itemSetPage"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static string[] GetLogText<TEntity>(this IItemSetPage<TEntity> itemSetPage)
        {
            return new []
            {
                $"Count: {itemSetPage.Count}",
                $"Page: {itemSetPage.Page}",
                $"Take: {itemSetPage.Take}",
                $"TotalCount: {itemSetPage.TotalCount}"
            };
        }

        /// <summary>
        /// Pulls default logging text from this object.
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <returns></returns>
        public static string[] GetLogText(this IItemSet<IFailure> validationErrors)
        {
            var errorStrings = new []
                {
                    $"Count: {validationErrors.Count}",
                    "ValidationErrors:"
                }
                .Concat(validationErrors.Items.Select(error => error.Message))
                .ToArray();
            return errorStrings;
        }
    }
}
