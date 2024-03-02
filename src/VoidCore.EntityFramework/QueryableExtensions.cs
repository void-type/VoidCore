using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public static async Task<IItemSet<T>> ToItemSet<T>(this IQueryable<T> query, PaginationOptions paginationOptions, CancellationToken cancellationToken)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .GetPage(paginationOptions)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return items.ToItemSet(paginationOptions, totalCount);
    }
}
