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
        public AuditUpdater(IDateTimeService now, ICurrentUserAccessor currentUser)
        {
            _now = now;
            _currentUser = currentUser;
        }

        /// <inheritdoc/>
        public void Create(IAuditable entity)
        {
            entity.CreatedOn = _now.Moment;
            entity.CreatedBy = _currentUser.Name;
            Update(entity);
        }

        /// <inheritdoc/>
        public void Update(IAuditable entity)
        {
            entity.ModifiedOn = _now.Moment;
            entity.ModifiedBy = _currentUser.Name;
        }

        private readonly ICurrentUserAccessor _currentUser;
        private readonly IDateTimeService _now;
    }
}
