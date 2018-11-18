using VoidCore.Model.ClientApp;
using VoidCore.Model.Time;

namespace VoidCore.Model.Data
{
    /// <inheritdoc/>
    public class SoftDeleteUpdater : ISoftDeleteUpdater
    {
        /// <summary>
        /// Construct a new SoftDeleteUpdater.
        /// </summary>
        /// <param name="now">A datetime service that provides the time the entity was updated</param>
        /// <param name="currentUser">An accessor for the current user's properties</param>
        public SoftDeleteUpdater(IDateTimeService now, ICurrentUserAccessor currentUser)
        {
            _now = now;
            _currentUser = currentUser;
        }

        /// <inheritdoc/>
        public void Delete(ISoftDeletable entity)
        {
            entity.DeletedOn = _now.Moment;
            entity.DeletedBy = _currentUser.Name;
        }

        /// <inheritdoc/>
        public void UnDelete(ISoftDeletable entity)
        {
            entity.DeletedOn = null;
            entity.DeletedBy = null;
        }

        private readonly IDateTimeService _now;
        private readonly ICurrentUserAccessor _currentUser;
    }
}
