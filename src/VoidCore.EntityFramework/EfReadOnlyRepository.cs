using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Model.Data;
using VoidCore.Model.Logging;
using VoidCore.Model.Text;

namespace VoidCore.EntityFramework
{
    /// <summary>
    /// A generic read-only repository. Optimized for Entity Framework Contexts.
    /// Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the repository</typeparam>
    public class EfReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        private readonly string _repoTypeName;
        private readonly ILoggingStrategy _loggingStrategy;

        /// <summary>
        /// The inner DbContext
        /// </summary>
        protected DbContext Context { get; }

        /// <inheritdoc/>
        public EfReadOnlyRepository(DbContext context, ILoggingStrategy loggingStrategy)
        {
            Context = context;
            _repoTypeName = GetType().GetFriendlyTypeName();
            _loggingStrategy = loggingStrategy;
        }

        /// <inheritdoc/>
        public virtual Task<int> Count(IQuerySpecification<T> specification, CancellationToken cancellationToken)
        {
            return GetBaseQuery()
                .TagWith(GetTag(nameof(Count), specification))
                .ApplyEfSpecification(specification)
                .CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<Maybe<T>> Get(IQuerySpecification<T> specification, CancellationToken cancellationToken)
        {
            return await GetBaseQuery()
                .TagWith(GetTag(nameof(Get), specification))
                .ApplyEfSpecification(specification)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> List(IQuerySpecification<T> specification, CancellationToken cancellationToken)
        {
            return await GetBaseQuery()
                .TagWith(GetTag(nameof(List), specification))
                .ApplyEfSpecification(specification)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken)
        {
            return await GetBaseQuery()
                .TagWith(GetTag(nameof(ListAll)))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the base queryable set.
        /// </summary>
        protected virtual IQueryable<T> GetBaseQuery() => Context.Set<T>();

        private string GetTag(string method, IQuerySpecification<T>? specification = null)
        {
            var specName = specification != null ?
                specification.GetType().GetFriendlyTypeName() :
                string.Empty;

            return _loggingStrategy.Log($"EF query called from: {_repoTypeName}.{method}({specName})");
        }
    }
}
