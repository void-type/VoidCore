namespace VoidCore.Model.Data
{
    /// <summary>
    /// A service to update the audit information on a persisted entity.
    /// </summary>
    public interface IAuditUpdater
    {
        /// <summary>
        /// Update the created and modified information on the entity.
        /// </summary>
        /// <param name="entity">The entity to be created</param>
        void Create(IAuditable entity);

        /// <summary>
        /// Update the modified information on the entity.
        /// </summary>
        /// <param name="entity">The entity to be updated</param>
        void Update(IAuditable entity);
    }
}
