using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Data;

namespace VoidCore.EntityFramework;

/// <summary>
/// A generic read/write repository. Optimized for Entity Framework Contexts.
/// Adapted from https://github.com/dotnet-architecture/eShopOnWeb
/// </summary>
/// <typeparam name="T">The type of entity stored in the repository</typeparam>
public class EfWritableRepository<T> : EfReadOnlyRepository<T>, IWritableRepository<T> where T : class
{
    /// <inheritdoc/>
    public EfWritableRepository(DbContext context) : base(context) { }

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        Context.Set<T>().AddRange(entities);

        return Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(T entity, CancellationToken cancellationToken)
    {
        Context.Set<T>().Remove(entity);
        return Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        Context.Set<T>().RemoveRange(entities);
        return Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        Context.Entry(entity).State = EntityState.Modified;
        return Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        return Context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    protected override IQueryable<T> GetBaseQuery() => Context.Set<T>();
}
