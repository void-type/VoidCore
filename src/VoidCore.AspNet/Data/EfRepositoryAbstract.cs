using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Model.Data;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A base class for repositories that use specifications to define queries.
    /// Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the repository</typeparam>
    public abstract class EfRepositoryAbstract<T> : IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// The inner DbContext
        /// </summary>
        protected readonly DbContext Context;

        /// <inheritdoc/>
        protected EfRepositoryAbstract(DbContext context)
        {
            Context = context;
        }

        /// <inheritdoc/>
        public virtual async Task<Maybe<T>> Get(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery().ApplyEfSpecification(spec).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery().ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> List(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery().ApplyEfSpecification(spec).ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<int> Count(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery().ApplyEfSpecification(spec).CountAsync(cancellationToken);
        }

        /// <summary>
        /// Implement in concrete repositories to get the base queryable set.
        /// </summary>
        protected abstract IQueryable<T> GetBaseQuery();
    }
}
