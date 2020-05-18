using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        /// Count entities that match a specification.
        /// </summary>
        /// <param name="specification">The specification that describes entities to count</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task<int> Count(IQuerySpecification<T> specification, CancellationToken cancellationToken);

        /// <summary>
        /// Get the first entity that matches a specification.
        /// </summary>
        /// <param name="specification">The specification that describes the entity to get</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task<Maybe<T>> Get(IQuerySpecification<T> specification, CancellationToken cancellationToken);

        /// <summary>
        /// List all entities in the repository.
        /// Get the first entity that matches a specification and transform it to another type of entity.
        /// </summary>
        /// <param name="specification">The specification that describes the entity to get</param>
        /// <param name="transformation">A tranformation to convert the original entity to another</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        /// <typeparam name="TNew">The transformed entity</typeparam>
        Task<Maybe<TNew>> Get<TNew>(IQuerySpecification<T> specification, Expression<Func<T, TNew>> transformation, CancellationToken cancellationToken);

        /// <summary>
        /// List all entities that match a specification.
        /// </summary>
        /// <param name="specification">The specification that describes entities to get</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task<IReadOnlyList<T>> List(IQuerySpecification<T> specification, CancellationToken cancellationToken);

        /// <summary>
        /// List all entities that match a specification and transform them to another type of entity.
        /// </summary>
        /// <param name="specification">The specification that describes entities to get</param>
        /// <param name="transformation">A tranformation to convert the original entity to another</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        /// <typeparam name="TNew">The transformed entity</typeparam>
        Task<IReadOnlyList<TNew>> List<TNew>(IQuerySpecification<T> specification, Expression<Func<T, TNew>> transformation, CancellationToken cancellationToken);

        /// <summary>
        /// List all entities in the repository.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken);

        /// <summary>
        /// List all entities in the repository and transform them to another type of entity.
        /// </summary>
        /// <param name="transformation">A tranformation to convert the original entity to another</param>
        /// <param name="cancellationToken">The cancellation token to cancel the task</param>
        /// <typeparam name="TNew">The transformed entity</typeparam>
        Task<IReadOnlyList<TNew>> ListAll<TNew>(Expression<Func<T, TNew>> transformation, CancellationToken cancellationToken);
    }
}
