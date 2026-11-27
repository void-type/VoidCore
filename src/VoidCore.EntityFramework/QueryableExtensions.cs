using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VoidCore.Model.Guards;
using VoidCore.Model.Responses.Collections;

namespace VoidCore.EntityFramework;

/// <summary>
/// Extensions for Queryables.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Query for a paginated ItemSet. This will query the total count and the requested page of entities.
    /// </summary>
    /// <typeparam name="T">The type of entities to retrieve</typeparam>
    /// <param name="query">The base query. Filtered and sorted, but don't use skip or take yet.</param>
    /// <param name="paginationOptions">Options to control pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="InvalidOperationException">Pagination is enabled but the query has no ordering. Pages of an unordered query can repeat or skip rows.</exception>
    public static async Task<IItemSet<T>> ToItemSet<T>(this IQueryable<T> query, PaginationOptions paginationOptions, CancellationToken cancellationToken)
    {
        query.EnsureNotNull();
        paginationOptions.EnsureNotNull();

        if (paginationOptions.IsPagingEnabled && !query.IsOrdered())
        {
            throw new InvalidOperationException(
                "The query enables pagination without ordering. Add an OrderBy, ending with a unique property, so pages are deterministic.");
        }

        var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var items = await query
            .GetPage(paginationOptions)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return items.ToItemSet(paginationOptions, totalCount);
    }

    // Walks the chain of calls applied to the query (Where, Select, Include, TagWith, and so on) looking for an OrderBy. Only the
    // chain counts: an ordering inside a lambda, such as a sorted subquery in a projection, doesn't order the result.
    private static bool IsOrdered<T>(this IQueryable<T> query)
    {
        var expression = query.Expression;

        while (expression is MethodCallExpression call && call.Arguments.Count > 0)
        {
            if (call.Method.DeclaringType == typeof(Queryable) && call.Method.Name is
                nameof(Queryable.OrderBy) or nameof(Queryable.OrderByDescending) or nameof(Queryable.ThenBy) or nameof(Queryable.ThenByDescending))
            {
                return true;
            }

            expression = call.Arguments[0];
        }

        return false;
    }
}
