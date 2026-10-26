using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VoidCore.Model.Auth;

namespace VoidCore.EntityFramework;

/// <summary>
/// Sets soft-delete properties on <see cref="VoidCore.Model.Data.ISoftDeletable"/> entities when a DbContext saves.
/// Set IsDeleted to true to soft-delete an entity and DeletedOn (UTC) and DeletedBy are recorded. Set IsDeleted to false to restore it and they are cleared.
/// Remove is still a normal, permanent delete. Set <see cref="RequireRecycleBin"/> to make it fail for entities that haven't been soft-deleted first.
/// Register as a singleton and add to the context's options with AddInterceptors.
/// To hide soft-deleted entities from queries, call ApplySoftDeleteQueryFilter in OnModelCreating.
/// </summary>
public class SoftDeletableSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    /// <summary>
    /// Create a new interceptor.
    /// </summary>
    /// <param name="timeProvider">A provider of the current time</param>
    /// <param name="currentUserAccessor">An accessor for the current user's name</param>
    public SoftDeletableSaveChangesInterceptor(TimeProvider timeProvider, ICurrentUserAccessor currentUserAccessor)
    {
        _timeProvider = timeProvider;
        _currentUserAccessor = currentUserAccessor;
    }

    /// <summary>
    /// When true, saving throws if an <see cref="VoidCore.Model.Data.ISoftDeletable"/> entity is deleted that wasn't already soft-deleted in the database.
    /// This makes the recycle bin mandatory: soft-delete and save first, then delete to permanently remove it.
    /// </summary>
    public bool RequireRecycleBin { get; init; }

    /// <inheritdoc/>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetSoftDeletableProperties(eventData.Context);
        return result;
    }

    /// <inheritdoc/>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetSoftDeletableProperties(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private void SetSoftDeletableProperties(DbContext? context)
    {
        context?.ChangeTracker.Entries().SetSoftDeletableProperties(_timeProvider, _currentUserAccessor.GetUserName(), RequireRecycleBin);
    }
}
