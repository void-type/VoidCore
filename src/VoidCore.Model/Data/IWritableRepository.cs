using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// A persistent repository that can be modified asynchronously. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored</typeparam>
    public interface IWritableRepository<T> : IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// Add an entity to the repository. Returns the entity after it has been added.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        Task<T> Add(T entity);

        /// <summary>
        /// Add many entities to the repository.
        /// </summary>
        /// <param name="entities">Entities to add</param>
        Task AddRange(IEnumerable<T> entities);

        /// <summary>
        /// The entity to update within the repository.
        /// </summary>
        /// <param name="entity">The altered entity to update</param>
        Task Update(T entity);

        /// <summary>
        /// Update many entities in the repository.
        /// </summary>
        /// <param name="entities">The altered entities to update</param>
        Task UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Remove an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        Task Remove(T entity);

        /// <summary>
        /// Remove many entities from the repository
        /// </summary>
        /// <param name="entities">The entities to remove</param>
        Task RemoveRange(IEnumerable<T> entities);
    }
}
