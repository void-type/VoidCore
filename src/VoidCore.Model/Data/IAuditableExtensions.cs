using System;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// Extensions to set audit properties.
    /// </summary>
    public static class IAuditableExtensions
    {
        /// <summary>
        /// Set audit properties on creation. Sets modified to equal creation properties.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="now">When the entity was created</param>
        /// <param name="currentUserName">The user who created the entity</param>
        public static void SetAuditCreated(this IAuditable entity, DateTime now, string currentUserName)
        {
            entity.CreatedOn = now;
            entity.CreatedBy = currentUserName;
            entity.ModifiedOn = now;
            entity.ModifiedBy = currentUserName;
        }

        /// <summary>
        /// Sets audit properties on modification.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="now">When the entity was modifed</param>
        /// <param name="currentUserName">The user who modified the entity</param>
        public static void SetAuditModified(this IAuditable entity, DateTime now, string currentUserName)
        {
            entity.ModifiedOn = now;
            entity.ModifiedBy = currentUserName;
        }
    }
}
