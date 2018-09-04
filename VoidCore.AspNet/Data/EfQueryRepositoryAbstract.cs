using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A readonly repository to obtain queries from a DbContext. Queries are used for views in Dotnet Core. This adds awareness of the DbContext
    /// concrete type because there is not generic method like Set for queries.
    /// </summary>
    /// <typeparam name="TDbEntity">The type of entities in the repository</typeparam>
    /// <typeparam name="TDbContext">The concrete type of DbContext to pull the DbQuery from</typeparam>
    public abstract class EfQueryRepositoryAbstract<TDbEntity, TDbContext> : EfReadOnlyRepository<TDbEntity> where TDbEntity : class, new() where TDbContext : DbContext

    {
        /// <summary>
        /// Override this to return the DbQuery set from the context. You can also add AsNoTracking() as needed.
        /// </summary>
        /// <value></value>
        public abstract override IQueryable<TDbEntity> Stored { get; }

        /// <summary>
        /// Construct a new repository.
        /// </summary>
        /// <param name="context">The inner DbContext</param>
        protected EfQueryRepositoryAbstract(TDbContext context) : base(context) { }
    }
}
