using Microsoft.EntityFrameworkCore;
using System.Linq;
using VoidCore.Model.Queries;

namespace VoidCore.AspNet.Queries
{
    /// <summary>
    /// Build queries from specifications.
    /// Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecificationEvaluator<T> where T : class
    {
        /// <summary>
        /// Evaluate the specification and build the query against the input.
        /// </summary>
        /// <param name="inputQuery">The input query</param>
        /// <param name="spec">The specification to evaluate</param>
        /// <returns>The final query</returns>
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, IQuerySpecification<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                foreach (var criteria in spec.Criteria)
                {
                    query = query.Where(criteria);
                }
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                var page = spec.Page > 1 ? spec.Page : 1;
                var take = spec.Take > 0 ? spec.Take : 0;

                query = query
                    .Skip((page - 1) * take)
                    .Take(take);
            }

            return query;
        }
    }
}
