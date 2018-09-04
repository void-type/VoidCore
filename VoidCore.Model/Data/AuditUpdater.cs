using VoidCore.Model.ClientApp;
using VoidCore.Model.Time;

namespace VoidCore.Model.Data
{
    /// <inheritdoc/>
    public class AuditUpdater : IAuditUpdater
    {
        /// <summary>
        /// Create a new audit updater
        /// </summary>
        /// <param name="now">A datetime service that provides the time the entity was updated</param>
        /// <param name="currentUser">An accessor for the current user's properties</param>
        public AuditUpdater(IDateTimeService now, ICurrentUser currentUser)
        {
            _now = now;
            _currentUser = currentUser;
        }

        /// <inheritdoc/>
        public void Create(IAuditable auditableEntity)
        {
            auditableEntity.CreatedOn = _now.Moment;
            auditableEntity.CreatedBy = _currentUser.Name;
            Update(auditableEntity);
        }

        /// <inheritdoc/>
        public void Update(IAuditable auditableEntity)
        {
            auditableEntity.ModifiedOn = _now.Moment;
            auditableEntity.ModifiedBy = _currentUser.Name;
        }

        private readonly ICurrentUser _currentUser;
        private readonly IDateTimeService _now;
    }
}
