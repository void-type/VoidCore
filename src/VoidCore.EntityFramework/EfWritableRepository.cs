using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Data;
using VoidCore.Model.Logging;

namespace VoidCore.EntityFramework
{
    /// <summary>
    /// A generic read/write repository. Optimized for Entity Framework Contexts.
    /// Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the repository</typeparam>
    public class EfWritableRepository<T> : EfReadOnlyRepository<T>, IWritableRepository<T> where T : class
    {
        /// <inheritdoc/>
        public EfWritableRepository(DbContext context, ILoggingStrategy loggingStrategy) : base(context, loggingStrategy) { }

        /// <inheritdoc/>
        public virtual async Task<T> Add(T entity, CancellationToken cancellationToken)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task AddRange(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            Context.Set<T>().AddRange(entities);
            await Context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task Remove(T entity, CancellationToken cancellationToken)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            Context.Set<T>().RemoveRange(entities);
            await Context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task Update(T entity, CancellationToken cancellationToken)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            await Context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override IQueryable<T> GetBaseQuery() => Context.Set<T>();
    }
}
