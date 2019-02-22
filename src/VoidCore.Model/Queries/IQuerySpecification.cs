using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VoidCore.Model.Queries
{
    /// <summary>
    /// A specification describing a query such as shape and content of entities. Can be used against repositories.
    /// </summary>
    /// <typeparam name="T">The type of entity in the specification</typeparam>
    public interface IQuerySpecification<T>
    {
        /// <summary>
        /// Criteria to filter the query.
        /// </summary>
        Expression<Func<T, bool>>[] Criteria { get; }

        /// <summary>
        /// Selectors for related entities to include
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Strings for related entities to include
        /// </summary>
        List<string> IncludeStrings { get; }

        /// <summary>
        /// Ascending sort on the query
        /// </summary>
        Expression<Func<T, object>> OrderBy { get; }

        /// <summary>
        /// Descending sort on the query
        /// </summary>
        Expression<Func<T, object>> OrderByDescending { get; }

        /// <summary>
        /// The page number if paging is enabled.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// The page size if paging is enabled.
        /// </summary>
        int Take { get; }

        /// <summary>
        /// Enable pagination on the query.
        /// </summary>
        bool IsPagingEnabled { get; }
    }
}
