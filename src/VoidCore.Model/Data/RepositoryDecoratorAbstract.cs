using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Model.Queries;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// A base class for altering repository behavior.
    /// </summary>
    /// <typeparam name="T">The type of entity in the repository</typeparam>
    public abstract class RepositoryDecoratorAbstract<T> : IWritableRepository<T> where T : class
    {
        /// <summary>
        /// The repository being decorated.
        /// </summary>
        protected readonly IWritableRepository<T> InnerRepository;

        /// <summary>
        /// Create a new repository decorator.
        /// </summary>
        /// <param name="innerRepository">The inner repository</param>
        public RepositoryDecoratorAbstract(IWritableRepository<T> innerRepository)
        {
            InnerRepository = innerRepository;
        }

        /// <inheritdoc/>
        public virtual Task<T> Add(T entity, CancellationToken cancellationToken = default)
        {
            return InnerRepository.Add(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return InnerRepository.AddRange(entities, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<int> Count(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return InnerRepository.Count(spec, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<Maybe<T>> Get(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return InnerRepository.Get(spec, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<IReadOnlyList<T>> List(IQuerySpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return InnerRepository.List(spec, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken = default)
        {
            return InnerRepository.ListAll(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task Remove(T entity, CancellationToken cancellationToken = default)
        {
            return InnerRepository.Remove(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return InnerRepository.RemoveRange(entities, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task Update(T entity, CancellationToken cancellationToken = default)
        {
            return InnerRepository.Update(entity, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return InnerRepository.UpdateRange(entities, cancellationToken);
        }
    }
}
