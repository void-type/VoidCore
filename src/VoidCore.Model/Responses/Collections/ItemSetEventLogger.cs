using Microsoft.Extensions.Logging;

namespace VoidCore.Model.Responses.Collections;

/// <summary>
/// Log meta information about the ItemSet.
/// </summary>
/// <typeparam name="TRequest">The request type of the event.</typeparam>
/// <typeparam name="TEntity">The type of items in the ItemSet</typeparam>
public class ItemSetEventLogger<TRequest, TEntity> : FallibleEventLoggerAbstract<TRequest, IItemSet<TEntity>>
{
    /// <inheritdoc cref="FallibleEventLoggerAbstract{TRequest, TResponse}"/>
    public ItemSetEventLogger(ILogger<ItemSetEventLogger<TRequest, TEntity>> logger) : base(logger) { }

    /// <inheritdoc/>
    protected override void OnSuccess(TRequest request, IItemSet<TEntity> response)
    {
        Logger.LogInformation("Responded with ItemSet. Count: {Count} IsPagingEnabled: {IsPagingEnabled} Page: {Page} Take: {Take} TotalCount: {TotalCount}",
            response.Count,
            response.IsPagingEnabled,
            response.Page,
            response.Take,
            response.TotalCount
        );

        base.OnSuccess(request, response);
    }
}
