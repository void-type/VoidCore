using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VoidCore.Model.Auth;

namespace VoidCore.EntityFramework;

/// <summary>
/// Sets audit properties on all added and modified <see cref="VoidCore.Model.Data.IAuditable"/> entities when a DbContext saves.
/// Times are recorded in UTC. Register as a singleton and add to the context's options with AddInterceptors.
/// </summary>
public class AuditableSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    /// <summary>
    /// Create a new interceptor.
    /// </summary>
    /// <param name="timeProvider">A provider of the current time</param>
    /// <param name="currentUserAccessor">An accessor for the current user's name</param>
    public AuditableSaveChangesInterceptor(TimeProvider timeProvider, ICurrentUserAccessor currentUserAccessor)
    {
        _timeProvider = timeProvider;
        _currentUserAccessor = currentUserAccessor;
    }

    /// <inheritdoc/>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditableProperties(eventData.Context);
        return result;
    }

    /// <inheritdoc/>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetAuditableProperties(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private void SetAuditableProperties(DbContext? context)
    {
        context?.ChangeTracker.Entries().SetAuditableProperties(_timeProvider, _currentUserAccessor.GetUserName());
    }
}
