using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A generic read-only repository backed by a DbQuery instead of a DbSet of entities. Optimized for Entity Framework
    /// Contexts. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the repository</typeparam>
    public class EfQueryRepository<T> : EfRepositoryAbstract<T> where T : class
    {
        /// <inheritdoc/>
        public EfQueryRepository(DbContext context) : base(context) { }

        /// <inheritdoc/>
        protected override IQueryable<T> GetBaseQuery() => Context.Query<T>();
    }
}
