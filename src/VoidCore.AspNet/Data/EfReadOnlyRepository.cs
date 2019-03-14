using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Model.Data;
using VoidCore.Model.Queries;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A generic read-only repository. Optimized for Entity Framework Contexts. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the repository</typeparam>
    public class EfReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// The inner DbContext
        /// </summary>
        protected readonly DbContext Context;

        /// <inheritdoc/>
        public EfReadOnlyRepository(DbContext context)
        {
            Context = context;
        }

        /// <inheritdoc/>
        public async Task<Maybe<T>> Get(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<T>> List(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec).ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<int> Count(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec).CountAsync(cancellationToken);
        }

        private IQueryable<T> ApplySpecification(IQuerySpecification<T> spec)
        {
            return SpecificationEvaluator.GetQuery(Context.Set<T>(), spec);
        }
    }
}
