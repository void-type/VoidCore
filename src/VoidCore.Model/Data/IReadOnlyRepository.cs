using System.Collections.Generic;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Model.Queries;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// A repository of entities that can be queried asynchronously.
    /// Adapted from https://github.com/dotnet-architecture/eShopOnWeb
    /// </summary>
    /// <typeparam name="T">The type of entity stored</typeparam>
    public interface IReadOnlyRepository<T> where T : class
    {

        /// <summary>
        /// Get the first entity that matches a specification.
        /// </summary>
        /// <param name="spec">The specification that describes the entity to get</param>
        Task<Maybe<T>> Get(IQuerySpecification<T> spec);

        /// <summary>
        /// List all entities in the repository.
        /// </summary>
        Task<IReadOnlyList<T>> ListAll();

        /// <summary>
        /// List al entities that match a specification.
        /// </summary>
        /// <param name="spec">The specification that describes entities to get</param>
        Task<IReadOnlyList<T>> List(IQuerySpecification<T> spec);

        /// <summary>
        /// Count entities that match a specification.
        /// </summary>
        /// <param name="spec">The specification that describes entities to count</param>
        Task<int> Count(IQuerySpecification<T> spec);
    }
}
