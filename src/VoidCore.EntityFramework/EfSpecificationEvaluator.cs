using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;

namespace VoidCore.EntityFramework;

/// <summary>
/// Build queries from specifications. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
/// </summary>
public static class EfSpecificationEvaluator
{
    /// <summary>
    /// Evaluate the specification and build the query against the input.
    /// </summary>
    /// <param name="inputQuery">The input query</param>
    /// <param name="specification">The specification to evaluate</param>
    /// <param name="countAll">Ignore pagination, include, and sort options for counting</param>
    /// <typeparam name="T">The type of entity to query</typeparam>
    /// <returns>The final query</returns>
    public static IQueryable<T> ApplyEfSpecification<T>(this IQueryable<T> inputQuery, IQuerySpecification<T> specification, bool countAll = false) where T : class
    {
        var query = inputQuery;

        query = specification.Criteria.Aggregate(query, (current, criteria) => current.Where(criteria));

        if (countAll)
        {
            return query.GetPage(PaginationOptions.None);
        }

        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        if (specification.Orderings.Count > 0)
        {
            var (orderBy, orderByIsDescending) = specification.Orderings[0];

            var orderedQuery = orderByIsDescending ?
                query.OrderByDescending(orderBy) :
                query.OrderBy(orderBy);

            var secondaries = specification.Orderings.Skip(1);

            foreach (var (thenBy, thenByIsDescending) in secondaries)
            {
                orderedQuery = thenByIsDescending ?
                    orderedQuery.ThenByDescending(thenBy) :
                    orderedQuery.ThenBy(thenBy);
            }

            query = orderedQuery;
        }

        return query.GetPage(specification.PaginationOptions);
    }
}
