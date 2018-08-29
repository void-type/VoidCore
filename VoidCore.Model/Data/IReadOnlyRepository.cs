using System.Linq;

namespace VoidCore.Model.Data
{
    /// <summary>
    /// A persistent repository of objects that can be queried.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IReadOnlyRepository<out TEntity> where TEntity : class
    {
        /// <summary>
        /// A getter for whole set of entities as a queryable collection.
        /// </summary>
        /// <value>The whole set of entities as a queryable collection.</value>
        IQueryable<TEntity> Stored { get; }
    }
}
