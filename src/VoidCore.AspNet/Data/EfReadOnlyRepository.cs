using Microsoft.EntityFrameworkCore;
using System.Linq;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A generic read-only repository. Optimized for Entity Framework Contexts. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the repository</typeparam>
    public class EfReadOnlyRepository<T> : EfRepositoryAbstract<T> where T : class
    {
        /// <inheritdoc/>
        public EfReadOnlyRepository(DbContext context, ILoggingStrategy loggingStrategy) : base(context, loggingStrategy) { }

        /// <inheritdoc/>
        protected override IQueryable<T> GetBaseQuery() => Context.Set<T>();
    }
}
