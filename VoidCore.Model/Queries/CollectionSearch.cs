using System;
using System.Linq;

namespace VoidCore.Model.Queries
{
    /// <summary>
    /// Perform common queries on properties of entities.
    /// </summary>
    public static class CollectionSearch
    {
        /// <summary>
        /// Split the search terms on any whitespace character, returning an array of terms containing non-whitespace characters.
        /// If searchText is null, will return a safe empty array.
        /// </summary>
        /// <param name="searchText">The raw search string to parse for terms</param>
        /// <param name="searchSeparator">The separator to split search terms. Default will split on all whitespace characters.</param>
        /// <returns></returns>
        public static string[] ParseStringForTerms(string searchText, char[] searchSeparator = null)
        {
            if (searchText == null)
            {
                return new string[0];
            }

            return searchText
                .Split(searchSeparator, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }

        /// <summary>
        /// Returns any entity that has all of the search terms in any selected property. This search is case insensitive.
        /// </summary>
        /// <param name="entities">The set of entities to search</param>
        /// <param name="searchTerms">An array of text terms to search by</param>
        /// <param name="propertySelectors">An array of selectors for each property to search of the entity</param>
        /// <typeparam name="TEntity">The type of entity to search</typeparam>
        /// <returns></returns>
        public static IQueryable<TEntity> SearchStringProperties<TEntity>(this IQueryable<TEntity> entities, string[] searchTerms, params Func<TEntity, string>[] propertySelectors)
        {
            return entities.Where(entity =>
                searchTerms.All(term =>
                    propertySelectors.Any(selector =>
                        selector.Invoke(entity) != null &&
                        selector.Invoke(entity).ToLower().Contains(term.ToLower()))));
        }

        /// <summary>
        /// Parses searchString for an array of search terms then performs SearchStringProperties.
        /// </summary>
        /// <param name="entities">The set of entities to search</param>
        /// <param name="searchString">A string containing whitespace delimited text terms to search by</param>
        /// <param name="propertySelectors">An array of selectors for each property to search of the entity</param>
        /// <typeparam name="TEntity">The type of entity to search</typeparam>
        /// <returns></returns>
        public static IQueryable<TEntity> SearchStringProperties<TEntity>(this IQueryable<TEntity> entities, string searchString, params Func<TEntity, string>[] propertySelectors)
        {
            var searchTerms = ParseStringForTerms(searchString);
            return SearchStringProperties(entities, searchTerms, propertySelectors);
        }
    }
}
