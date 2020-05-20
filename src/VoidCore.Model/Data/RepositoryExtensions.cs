using VoidCore.Model.Auth;
using VoidCore.Model.Time;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// Extension methods to decorate repositories with additional functionality.
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Decorate the repository's Add and Update methods with logic to update the audit properties of the entity.
        /// </summary>
        /// <param name="innerRepository">The repository to decorate.</param>
        /// <param name="now">A service to get the current datetime</param>
        /// <param name="currentUserAccessor">A service to get the current user</param>
        /// <typeparam name="T">The type of entity stored in the repository</typeparam>
        /// <returns>The decorated repository</returns>
        public static IWritableRepository<T> AddAuditability<T>(this IWritableRepository<T> innerRepository, IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        where T : class, IAuditable
        {
            return new AuditableRepositoryDecorator<T>(innerRepository, now, currentUserAccessor);
        }

        /// <summary>
        /// Decorate the repository's Remove methods with logic to mark the entity as deleted.
        /// </summary>
        /// <param name="innerRepository">The repository to decorate.</param>
        /// <param name="now">A service to get the current datetime</param>
        /// <param name="currentUserAccessor">A service to get the current user</param>
        /// <typeparam name="T">The type of entity stored in the repository</typeparam>
        /// <returns>The decorated repository</returns>
        public static IWritableRepository<T> AddSoftDeletability<T>(this IWritableRepository<T> innerRepository, IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        where T : class, ISoftDeletable
        {
            return new SoftDeletableRepositoryDecorator<T>(innerRepository, now, currentUserAccessor);
        }
    }
}
