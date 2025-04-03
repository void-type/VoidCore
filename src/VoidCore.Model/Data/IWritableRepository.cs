namespace VoidCore.Model.Data;

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
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task<T> Add(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Add many entities to the repository.
    /// </summary>
    /// <param name="entities">Entities to add</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task AddRange(IEnumerable<T> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Remove an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task Remove(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Remove many entities from the repository
    /// </summary>
    /// <param name="entities">The entities to remove</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken);

    /// <summary>
    /// The entity to update within the repository.
    /// </summary>
    /// <param name="entity">The altered entity to update</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task Update(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Update many entities in the repository.
    /// </summary>
    /// <param name="entities">The altered entities to update</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken);
}
