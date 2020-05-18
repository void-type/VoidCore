using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoidCore.Domain;
using VoidCore.Model.Configuration;
using VoidCore.Model.Data;
using VoidCore.Model.Logging;

namespace VoidCore.EntityFramework
{
    /// <summary>
    /// A generic read-only repository. Optimized for Entity Framework Contexts.
    /// Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the repository</typeparam>
    public class EfReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// The inner DbContext
        /// </summary>
        protected readonly DbContext Context;

        private readonly string _repoTypeName;
        private readonly ILoggingStrategy _loggingStrategy;

        /// <inheritdoc/>
        protected EfReadOnlyRepository(DbContext context, ILoggingStrategy loggingStrategy)
        {
            Context = context;
            _repoTypeName = GetType().GetFriendlyTypeName();
            _loggingStrategy = loggingStrategy;
        }

        /// <inheritdoc/>
        public virtual async Task<int> Count(IQuerySpecification<T> specification, CancellationToken cancellationToken)
        {
            return await GetBaseQuery()
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
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> List(IQuerySpecification<T> specification, CancellationToken cancellationToken)
        {
            return await GetBaseQuery()
                .TagWith(GetTag(nameof(List), specification))
                .ApplyEfSpecification(specification)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken)
        {
            return await GetBaseQuery()
                .TagWith(GetTag(nameof(ListAll)))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get the base queryable set.
        /// </summary>
        protected virtual IQueryable<T> GetBaseQuery() => Context.Set<T>();

        private string GetTag(string method, IQuerySpecification<T> specification = null)
        {
            var specName = specification != null ?
                specification.GetType().GetFriendlyTypeName() :
                string.Empty;

            return _loggingStrategy.Log($"EF query called from: {_repoTypeName}.{method}({specName})");
        }
    }
}
