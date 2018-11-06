using System.Linq;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// A repository of entities that can be queried.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity stored</typeparam>
    public interface IReadOnlyRepository<out TEntity> where TEntity : class
    {
        /// <summary>
        /// A getter for whole set of entities as a queryable collection.
        /// </summary>
        IQueryable<TEntity> Stored { get; }
    }
}
