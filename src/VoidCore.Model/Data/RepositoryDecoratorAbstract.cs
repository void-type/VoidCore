using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace VoidCore.Model.Data;

/// <summary>
/// A base class for altering repository behavior.
/// </summary>
/// <typeparam name="T">The type of entity in the repository</typeparam>
public abstract class RepositoryDecoratorAbstract<T> : IWritableRepository<T> where T : class
{
    /// <summary>
    /// The repository being decorated.
    /// </summary>
    protected IWritableRepository<T> InnerRepository { get; }

    /// <summary>
    /// Create a new repository decorator.
    /// </summary>
    /// <param name="innerRepository">The inner repository</param>
    protected RepositoryDecoratorAbstract(IWritableRepository<T> innerRepository)
    {
        InnerRepository = innerRepository;
    }

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        return await InnerRepository.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await InnerRepository.AddRangeAsync(entities, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(IQuerySpecification<T> specification, CancellationToken cancellationToken)
    {
        return await InnerRepository.CountAsync(specification, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<Maybe<T>> GetAsync(IQuerySpecification<T> specification, CancellationToken cancellationToken)
    {
        return await InnerRepository.GetAsync(specification, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IReadOnlyList<T>> ListAsync(IQuerySpecification<T> specification, CancellationToken cancellationToken)
    {
        return await InnerRepository.ListAsync(specification, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IItemSet<T>> ListPageAsync(IQuerySpecification<T> specification, CancellationToken cancellationToken)
    {
        return await InnerRepository.ListPageAsync(specification, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
    {
        return await InnerRepository.ListAllAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(T entity, CancellationToken cancellationToken)
    {
        await InnerRepository.RemoveAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await InnerRepository.RemoveRangeAsync(entities, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        await InnerRepository.UpdateAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await InnerRepository.UpdateRangeAsync(entities, cancellationToken);
    }
}
