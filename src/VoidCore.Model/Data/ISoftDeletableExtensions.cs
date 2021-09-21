using System;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// Extensions to set audit properties.
    /// </summary>
    public static class ISoftDeletableExtensions
    {
        /// <summary>
        /// Set soft-delete properties on deletion.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="now">When the entity was deleted</param>
        /// <param name="currentUserName">The user who deleted the entity</param>
        public static void SetSoftDeleted(this ISoftDeletable entity, DateTime now, string currentUserName)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = now;
            entity.DeletedBy = currentUserName;
        }
    }
}
