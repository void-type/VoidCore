using VoidCore.Model.Time;
using VoidCore.Model.Users;

namespace VoidCore.Model.Data
{
    /// <inheritdoc/>
    public class SoftDeleteUpdater : ISoftDeleteUpdater
    {
        /// <summary>
        /// Construct a new SoftDeleteUpdater.
        /// </summary>
        /// <param name="now">A datetime service that provides the time the entity was updated</param>
        /// <param name="currentUserAccessor">An accessor for the current user's properties</param>
        public SoftDeleteUpdater(IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        {
            _now = now;
            _currentUserAccessor = currentUserAccessor;
        }

        /// <inheritdoc/>
        public void Delete(ISoftDeletable entity)
        {
            entity.DeletedOn = _now.Moment;
            entity.DeletedBy = _currentUserAccessor.User.Name;
        }

        /// <inheritdoc/>
        public void UnDelete(ISoftDeletable entity)
        {
            entity.DeletedOn = null;
            entity.DeletedBy = null;
        }

        private readonly IDateTimeService _now;
        private readonly ICurrentUserAccessor _currentUserAccessor;
    }
}
