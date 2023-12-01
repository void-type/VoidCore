using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace VoidCore.Model.Data;

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
    /// List all entities that match a specification.
    /// </summary>
    /// <param name="specification">The specification that describes entities to get</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task<IReadOnlyList<T>> List(IQuerySpecification<T> specification, CancellationToken cancellationToken);

    /// <summary>
    /// Get a page from the list of all entities that match a specification.
    /// </summary>
    /// <param name="specification">The specification that describes entities to get</param>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task<IItemSet<T>> ListPage(IQuerySpecification<T> specification, CancellationToken cancellationToken);

    /// <summary>
    /// List all entities in the repository.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the task</param>
    Task<IReadOnlyList<T>> ListAll(CancellationToken cancellationToken);
}
