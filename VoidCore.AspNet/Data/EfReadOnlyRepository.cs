using Microsoft.EntityFrameworkCore;
using System.Linq;
using VoidCore.Model.Data;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A readonly repository to obtain entities from a DbContext.
    /// </summary>
    /// <typeparam name="TDbEntity">The type of entities in the repository</typeparam>
    public class EfReadOnlyRepository<TDbEntity> : IReadOnlyRepository<TDbEntity> where TDbEntity : class, new()

    {
        /// <summary>
        /// A getter for the whole queryable set. By default has AsNoTracking() enabled.
        /// </summary>
        /// <returns>The queryable set</returns>
        public virtual IQueryable<TDbEntity> Stored => Context.Set<TDbEntity>().AsNoTracking();

        /// <summary>
        /// The inner DbContext
        /// </summary>
        protected readonly DbContext Context;

        /// <summary>
        /// Construct a new repository.
        /// </summary>
        /// <param name="context">The inner DbContext</param>
        public EfReadOnlyRepository(DbContext context)
        {
            Context = context;
        }
    }
}
