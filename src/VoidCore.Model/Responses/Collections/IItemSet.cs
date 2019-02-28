using System.Collections.Generic;

namespace VoidCore.Model.Responses.Collections
{
    /// <summary>
    /// A set of items. Can optionally by a page of a full set.
    /// </summary>
    public interface IItemSet<out T>
    {
        /// <summary>
        /// The count of items in this set.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The items in this set.
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// When true, this is a page of a full set.
        /// </summary>
        bool IsPagingEnabled { get; }

        /// <summary>
        /// If paging is enabled, this represents the page number in the total set.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// If paging is enabled, the requested number of results per page.
        /// </summary>
        int Take { get; }

        /// <summary>
        /// The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
        /// </summary>
        int TotalCount { get; }
    }
}
