using System;
using System.Collections.Generic;

namespace VoidCore.Model.Queries
{
    /// <summary>
    /// A series of strings to use in a text-based search.
    /// </summary>
    public class SearchTerms
    {
        /// <summary>
        /// The terms to search for.
        /// </summary>
        public IEnumerable<string> Terms { get; }

        /// <summary>
        /// Create a new SearchTerms by directly supplying the terms.
        /// </summary>
        /// <param name="searchTerms"></param>
        public SearchTerms(IEnumerable<string> searchTerms)
        {
            Terms = searchTerms ?? new string[0];
        }

        /// <summary>
        /// Create a new SearchTerms by splitting a string using a separator.
        /// The string will be split on the separator and empty entries will be removed.
        /// </summary>
        /// <param name="searchString">The raw string where terms will be pulled from</param>
        /// <param name="searchTermSeparator">The separator to split the searchString by. When null, the string will be split on all whitespace.</param>
        public SearchTerms(string searchString, char[] searchTermSeparator = null)
        {
            Terms = searchString?.Split(searchTermSeparator, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
        }
    }
}
