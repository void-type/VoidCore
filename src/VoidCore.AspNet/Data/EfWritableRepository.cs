using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Data;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A repository to read, update and remove entities from a DbContext.
    /// </summary>
    /// <typeparam name="TDbEntity">The type of entities in the repository</typeparam>
    public class EfWritableRepository<TDbEntity> : EfReadOnlyRepository<TDbEntity>, IWritableRepository<TDbEntity> where TDbEntity : class, new()
    {
        /// <inheritdoc/>
        public override IQueryable<TDbEntity> Stored => Context.Set<TDbEntity>();

        /// <inheritdoc/>
        public TDbEntity New => new TDbEntity();

        /// <summary>
        /// Construct a new EfWritableRepository.
        /// </summary>
        /// <param name="context">The inner DbContext</param>
        public EfWritableRepository(DbContext context) : base(context) { }

        /// <inheritdoc/>
        public void Add(TDbEntity entity)
        {
            Context.Set<TDbEntity>().Add(entity);
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<TDbEntity> entities)
        {
            Context.Set<TDbEntity>().AddRange(entities);
        }

        /// <inheritdoc/>
        public void Remove(TDbEntity entity)
        {
            Context.Set<TDbEntity>().Remove(entity);
        }

        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<TDbEntity> entities)
        {
            Context.Set<TDbEntity>().RemoveRange(entities);
        }
    }
}
