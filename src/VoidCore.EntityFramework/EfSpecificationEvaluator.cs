using Microsoft.EntityFrameworkCore;
using System.Linq;
using VoidCore.Model.Data;

namespace VoidCore.EntityFramework
{
    /// <summary>
    /// Build queries from specifications. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    internal static class EfSpecificationEvaluator
    {
        /// <summary>
        /// Evaluate the specification and build the query against the input.
        /// </summary>
        /// <param name="inputQuery">The input query</param>
        /// <param name="specification">The specification to evaluate</param>
        /// <typeparam name="T">The type of entity to query</typeparam>
        /// <returns>The final query</returns>
        internal static IQueryable<T> ApplyEfSpecification<T>(this IQueryable<T> inputQuery, IQuerySpecification<T> specification) where T : class
        {
            var query = inputQuery;

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            query = specification.Criteria.Aggregate(query, (current, criteria) => current.Where(criteria));

            if (specification.OrderBy.HasValue)
            {
                query = query
                    .OrderBy(specification.OrderBy.Value)
                    .ApplySecondaryOrderings(specification);
            }
            else if (specification.OrderByDescending.HasValue)
            {
                query = query
                    .OrderByDescending(specification.OrderByDescending.Value)
                    .ApplySecondaryOrderings(specification);
            }

            var paginationOptions = specification.PaginationOptions;

            if (paginationOptions.IsPagingEnabled)
            {
                query = query
                    .Skip((paginationOptions.Page - 1) * paginationOptions.Take)
                    .Take(paginationOptions.Take);
            }

            return query;
        }

        private static IQueryable<T> ApplySecondaryOrderings<T>(this IOrderedQueryable<T> query, IQuerySpecification<T> specification) where T : class
        {
            foreach (var (thenBy, isDescending) in specification.SecondaryOrderings)
            {
                query = isDescending ?
                    query.ThenByDescending(thenBy) :
                    query.ThenBy(thenBy);
            }

            return query;
        }
    }
}
