using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using VoidCore.Model.Data;
using VoidCore.Model.Time;

namespace VoidCore.EntityFramework;

/// <summary>
/// Extensions for EF Entity Entries.
/// </summary>
public static class EntityEntryExtensions
{
    /// <summary>
    /// Set auditable and soft-delete properties on all entries.
    /// </summary>
    /// <param name="entries">The change tracking entries</param>
    /// <param name="dateTimeService">A datetime service</param>
    /// <param name="user">The user name</param>
    public static void SetAuditableProperties(this IEnumerable<EntityEntry> entries, IDateTimeService dateTimeService, string user)
    {
        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditable auditableEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableEntity.SetAuditCreated(dateTimeService.Moment, user);
                        break;
                    case EntityState.Modified:
                        auditableEntity.SetAuditModified(dateTimeService.Moment, user);
                        break;
                }
            }

            if (entry.Entity is IAuditableWithOffset auditableWithOffsetEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableWithOffsetEntity.SetAuditCreated(dateTimeService.Moment, user);
                        break;
                    case EntityState.Modified:
                        auditableWithOffsetEntity.SetAuditModified(dateTimeService.Moment, user);
                        break;
                }
            }
        }
    }
}
