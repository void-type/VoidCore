using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// A repository of entities that can be queried asynchronously. Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored</typeparam>
    public interface IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// Get the first entity that matches a specification.
        /// </summary>
        /// <param name="spec">The specification that describes the entity to get</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task<Maybe<T>> Get(IQuerySpecification<T> spec, CancellationToken cancellationToken);

        /// <summary>
        /// List all entities in the repository.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken);

        /// <summary>
        /// List al entities that match a specification.
        /// </summary>
        /// <param name="spec">The specification that describes entities to get</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task<IReadOnlyList<T>> List(IQuerySpecification<T> spec, CancellationToken cancellationToken);

        /// <summary>
        /// Count entities that match a specification.
        /// </summary>
        /// <param name="spec">The specification that describes entities to count</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task<int> Count(IQuerySpecification<T> spec, CancellationToken cancellationToken);
    }
}
