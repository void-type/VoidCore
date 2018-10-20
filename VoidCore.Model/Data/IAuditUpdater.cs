namespace VoidCore.Model.Data
{
    /// <summary>
    /// A service to update the audit information on a peristed entity.
    /// </summary>
    public interface IAuditUpdater
    {
        /// <summary>
        /// Update the created and modified information on the entity.
        /// </summary>
        /// <param name="auditableEntity">The entity to be created</param>
        void Create(IAuditable auditableEntity);

        /// <summary>
        /// Update the modified information on the entity.
        /// </summary>
        /// <param name="auditableEntity">The entity to be updated</param>
        void Update(IAuditable auditableEntity);
    }
}
