using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoidCore.Model.Data;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A generic read/write repository. Optimized for Entity Framework Contexts. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EfWritableRepository<T> : EfReadOnlyRepository<T>, IWritableRepository<T> where T : class
    {
        /// <inheritdoc/>
        public EfWritableRepository(DbContext context) : base(context) { }

        /// <inheritdoc/>
        public async Task<T> Add(T entity)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public async Task AddRange(IEnumerable<T> entities)
        {
            Context.Set<T>().AddRange(entities);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            await Context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}
