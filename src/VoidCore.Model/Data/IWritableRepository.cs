using System.Collections.Generic;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// A persistent repository that can be modified.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity stored</typeparam>
    public interface IWritableRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Returns a new default instance of the entity type. Does not add the returned entity to the repository.
        /// </summary>
        TEntity New { get; }

        /// <summary>
        /// Add a single entity to the repository.
        /// </summary>
        /// <param name="entity">An entity</param>
        void Add(TEntity entity);

        /// <summary>
        /// Add multiple entities at once to the repository.
        /// </summary>
        /// <param name="entities">A collection of entities</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Remove one entity from the repository.
        /// </summary>
        /// <param name="entity">An entity</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Remove multiple entities at once from the repository.
        /// </summary>
        /// <param name="entities">A collection of entities</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
