using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VoidCore.Model.Data;

namespace VoidCore.EntityFramework;

/// <summary>
/// Extensions for EF Entity Entries.
/// </summary>
public static class EntityEntryExtensions
{
    /// <summary>
    /// Set auditable properties on all added and modified entries. Times are recorded in UTC.
    /// </summary>
    /// <param name="entries">The change tracking entries</param>
    /// <param name="timeProvider">A provider of the current time</param>
    /// <param name="user">The user name</param>
    public static void SetAuditableProperties(this IEnumerable<EntityEntry> entries, TimeProvider timeProvider, string user)
    {
        var now = timeProvider.GetUtcNow();

        foreach (var entry in entries)
        {
            if (entry.Entity is not IAuditable auditableEntity)
            {
                continue;
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    auditableEntity.SetAuditCreated(now, user);
                    break;
                case EntityState.Modified:
                    auditableEntity.SetAuditModified(now, user);
                    break;
            }
        }
    }

    /// <summary>
    /// Set soft-deletable properties on all added and modified entries. DeletedOn (UTC) and DeletedBy are recorded when IsDeleted is true and
    /// DeletedOn isn't already set, and cleared when IsDeleted is false. Soft-deleting modifies the entity, so <see cref="SetAuditableProperties"/>
    /// records that too, in either order.
    /// </summary>
    /// <param name="entries">The change tracking entries</param>
    /// <param name="timeProvider">A provider of the current time</param>
    /// <param name="user">The user name</param>
    /// <param name="requireRecycleBin">Throw if an entity is deleted that wasn't already soft-deleted in the database</param>
    /// <exception cref="InvalidOperationException">An entity is deleted without being soft-deleted first and <paramref name="requireRecycleBin"/> is true</exception>
    public static void SetSoftDeletableProperties(this IEnumerable<EntityEntry> entries, TimeProvider timeProvider, string user, bool requireRecycleBin = false)
    {
        var now = timeProvider.GetUtcNow();

        foreach (var entry in entries)
        {
            if (entry.Entity is not ISoftDeletable entity)
            {
                continue;
            }

            switch (entry.State)
            {
                case EntityState.Added or EntityState.Modified:
                    if (entity.IsDeleted)
                    {
                        if (entity.DeletedOn is null)
                        {
                            entity.DeletedOn = now;
                            entity.DeletedBy = user;
                        }
                    }
                    else if (entity.DeletedOn is not null || !string.IsNullOrEmpty(entity.DeletedBy))
                    {
                        entity.DeletedOn = null;
                        entity.DeletedBy = string.Empty;
                    }

                    break;

                case EntityState.Deleted when requireRecycleBin && entry.Property(nameof(ISoftDeletable.IsDeleted)).OriginalValue is not true:
                    throw new InvalidOperationException(
                        $"{entry.Metadata.ClrType.Name} can't be deleted until it has been soft-deleted and saved. Set IsDeleted to true first, then delete it to remove it permanently.");
            }
        }
    }
}
