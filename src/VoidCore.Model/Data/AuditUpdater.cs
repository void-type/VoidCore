using VoidCore.Model.Time;
using VoidCore.Model.Users;

namespace VoidCore.Model.Data
{
    /// <inheritdoc/>
    public class AuditUpdater : IAuditUpdater
    {
        /// <summary>
        /// Create a new audit updater
        /// </summary>
        /// <param name="now">A datetime service that provides the time the entity was updated</param>
        /// <param name="currentUserAccessor">An accessor for the current user's properties</param>
        public AuditUpdater(IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        {
            _now = now;
            _currentUserAccessor = currentUserAccessor;
        }

        /// <inheritdoc/>
        public void Create(IAuditable entity)
        {
            entity.CreatedOn = _now.Moment;
            entity.CreatedBy = _currentUserAccessor.User.Name;
            Update(entity);
        }

        /// <inheritdoc/>
        public void Update(IAuditable entity)
        {
            entity.ModifiedOn = _now.Moment;
            entity.ModifiedBy = _currentUserAccessor.User.Name;
        }

        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IDateTimeService _now;
    }
}
