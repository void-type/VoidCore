using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.AspNet.Queries;
using VoidCore.Domain;
using VoidCore.Model.Data;
using VoidCore.Model.Queries;

namespace VoidCore.AspNet.Data
{
    /// <summary>
    /// A generic read-only repository. Optimized for Entity Framework Contexts.
    /// Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        public virtual async Task<Maybe<T>> Get(IQuerySpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<T>> ListAll()
        {
            return await Context.Set<T>().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<T>> List(IQuerySpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<int> Count(IQuerySpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(IQuerySpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(Context.Set<T>(), spec);
        }
    }
}
