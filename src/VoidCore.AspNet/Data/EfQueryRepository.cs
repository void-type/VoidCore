using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VoidCore.Model.Logging;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A generic read-only repository backed by a DbQuery instead of a DbSet of entities. Optimized for Entity Framework
    /// Contexts. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the repository</typeparam>
    [Obsolete("Query sets are obsolete in .Net Core 3.0. Use DbSet<> with .HasNoKey() in your context to map a view.")]
    public class EfQueryRepository<T> : EfRepositoryAbstract<T> where T : class
    {
        /// <inheritdoc/>
        public EfQueryRepository(DbContext context, ILoggingStrategy loggingStrategy) : base(context, loggingStrategy) { }

        /// <inheritdoc/>
        protected override IQueryable<T> GetBaseQuery() => Context.Query<T>();
    }
}
