using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using VoidCore.Model.Data;
using VoidCore.Model.Time;

namespace VoidCore.EntityFramework
{
    /// <summary>
    /// Extensions for EF Entity Entries.
    /// </summary>
    public static class EntityEntryEnumerableExtensions
    {
        /// <summary>
        /// A convenience method to set all auditable and soft-delete properties from within a DBContext.
        /// </summary>
        /// <param name="entries">The change tracking entries</param>
        /// <param name="dateTimeService">A datetime service</param>
        /// <param name="user">The user name</param>
        public static void SetAllAuditableProperties(this IEnumerable<EntityEntry> entries, IDateTimeService dateTimeService, string user)
        {
            entries.SetAuditableProperties(dateTimeService.Moment, user);
            entries.SetAuditableWithOffsetProperties(dateTimeService.MomentWithOffset, user);
        }

        /// <summary>
        /// Set auditable and soft-delete properties on all entries.
        /// </summary>
        /// <param name="entries">The change tracking entries</param>
        /// <param name="now">The current datetime</param>
        /// <param name="user">The user name</param>
        public static void SetAuditableProperties(this IEnumerable<EntityEntry> entries, DateTime now, string user)
        {
            foreach (var entry in entries)
            {
                if (entry.Entity is ISoftDeletable softDeletableEntity && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    softDeletableEntity.SetSoftDeleted(now, user);
                    continue;
                }

                if (entry.Entity is IAuditable auditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.SetAuditCreated(now, user);
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        auditableEntity.SetAuditModified(now, user);
                    }
                }
            }
        }

        /// <summary>
        /// Set auditable and soft-delete (with DateTimeOffset) properties on all entries.
        /// </summary>
        /// <param name="entries">The change tracking entries</param>
        /// <param name="now">The current datetime</param>
        /// <param name="user">The user name</param>
        public static void SetAuditableWithOffsetProperties(this IEnumerable<EntityEntry> entries, DateTimeOffset now, string user)
        {
            foreach (var entry in entries)
            {
                if (entry.Entity is ISoftDeletableWithOffset softDeletableEntity && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    softDeletableEntity.SetSoftDeleted(now, user);
                    continue;
                }

                if (entry.Entity is IAuditableWithOffset auditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.SetAuditCreated(now, user);
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        auditableEntity.SetAuditModified(now, user);
                    }
                }
            }
        }
    }
}
