using System;
using System.Linq;
using System.Linq.Expressions;

namespace VoidCore.Model.Queries
{
    /// <summary>
    /// Build expressions for searching.
    /// </summary>
    public static class SearchCriteria
    {
        /// <summary>
        /// Check entities string properties and return entities that contain all terms in any one of their properties. Search is not case-sensitive.
        /// </summary>
        /// <param name="searchTerms">Terms to search for</param>
        /// <param name="propertySelectors">Selectors for the properties to be searched</param>
        /// <typeparam name="T">The type of entity to search</typeparam>
        public static Expression<Func<T, bool>> PropertiesContainAll<T>(SearchTerms searchTerms, params Func<T, string>[] propertySelectors)
        {
            return entity =>
                searchTerms.Terms.All(term =>
                    propertySelectors.Any(selector =>
                        selector(entity) != null &&
                        selector(entity).ToLower().Contains(term.ToLower())));
        }

        /// <summary>
        /// Check entities string properties and return entities that contain any term in any of their properties. Search is not case-sensitive.
        /// </summary>
        /// <param name="searchTerms">Terms to search for</param>
        /// <param name="propertySelectors">Selectors for the properties to be searched</param>
        /// <typeparam name="T">The type of entity to search</typeparam>
        public static Expression<Func<T, bool>> PropertiesContainAny<T>(SearchTerms searchTerms, params Func<T, string>[] propertySelectors)
        {
            return entity =>
                searchTerms.Terms.Any(term =>
                    propertySelectors.Any(selector =>
                        selector(entity) != null &&
                        selector(entity).ToLower().Contains(term.ToLower())));
        }
    }
}
