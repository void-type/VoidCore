using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Model.Data;
using VoidCore.Model.Logging;

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

#if NETCOREAPP2_2
        private string _repoTypeName;
        private ILoggingStrategy _loggingStrategy;
#endif

        /// <inheritdoc/>
        protected EfRepositoryAbstract(DbContext context, ILoggingStrategy loggingStrategy)
        {
            Context = context;
#if NETCOREAPP2_2
            _repoTypeName = this.GetType().GetFriendlyTypeName();
            _loggingStrategy = loggingStrategy;
#endif
        }

        /// <inheritdoc/>
        public virtual async Task<Maybe<T>> Get(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery()
#if NETCOREAPP2_2
                .TagWith(GetTag(nameof(Get), spec))
#endif
                .ApplyEfSpecification(spec)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery()
#if NETCOREAPP2_2
                .TagWith(GetTag(nameof(ListAll)))
#endif
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> List(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery()
#if NETCOREAPP2_2
                .TagWith(GetTag(nameof(List), spec))
#endif
                .ApplyEfSpecification(spec)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<int> Count(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery()
#if NETCOREAPP2_2
                .TagWith(GetTag(nameof(Count), spec))
#endif
                .ApplyEfSpecification(spec)
                .CountAsync(cancellationToken);
        }

        /// <summary>
        /// Implement in concrete repositories to get the base queryable set.
        /// </summary>
        protected abstract IQueryable<T> GetBaseQuery();

#if NETCOREAPP2_2
        private string GetTag(string method, IQuerySpecification<T> spec = null)
        {
            var specName = spec != null ?
                spec.GetType().GetFriendlyTypeName() :
                string.Empty;

            return _loggingStrategy.Log($"EF query called from: {_repoTypeName}.{method}({specName})");
        }
#endif
    }
}
