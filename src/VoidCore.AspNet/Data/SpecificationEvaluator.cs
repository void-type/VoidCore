using Microsoft.EntityFrameworkCore;
using System.Linq;
using VoidCore.Model.Queries;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// Build queries from specifications. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpecificationEvaluator<T> where T : class
    {
        /// <summary>
        /// Evaluate the specification and build the query against the input.
        /// </summary>
        /// <param name="inputQuery">The input query</param>
        /// <param name="specification">The specification to evaluate</param>
        /// <returns>The final query</returns>
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, IQuerySpecification<T> specification)
        {
            var query = inputQuery;

            if (specification.Criteria != null)
            {
                foreach (var criteria in specification.Criteria)
                {
                    query = query.Where(criteria);
                }
            }

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPagingEnabled)
            {
                query = query
                    .Skip((specification.Page - 1) * specification.Take)
                    .Take(specification.Take);
            }

            return query;
        }
    }
}
