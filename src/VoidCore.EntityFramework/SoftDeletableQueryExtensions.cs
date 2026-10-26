using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace VoidCore.EntityFramework;

/// <summary>
/// Extensions to hide soft-deleted entities from queries and to include them again for operations like a recycle bin.
/// </summary>
public static class SoftDeletableQueryExtensions
{
    /// <summary>
    /// The name of the query filter that hides soft-deleted entities.
    /// </summary>
    public const string QueryFilterName = "SoftDelete";

    /// <summary>
    /// Hide soft-deleted entities from all queries of every <see cref="ISoftDeletable"/> entity type in the model.
    /// Call this last in OnModelCreating so it sees every entity type. EF Core doesn't allow mixing named and unnamed query filters
    /// on one entity type, so any other query filters on soft-deletable entities must be named, for example HasQueryFilter("Tenant", ...).
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    /// <returns>The model builder</returns>
    public static ModelBuilder ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        var rootEntityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(t => t.BaseType is null && !t.IsOwned() && typeof(ISoftDeletable).IsAssignableFrom(t.ClrType))
            .ToList();

        foreach (var entityType in rootEntityTypes)
        {
            var entity = Expression.Parameter(entityType.ClrType, "e");
            var isNotDeleted = Expression.Not(Expression.Property(entity, nameof(ISoftDeletable.IsDeleted)));

            entityType.SetQueryFilter(QueryFilterName, Expression.Lambda(isNotDeleted, entity));
        }

        return modelBuilder;
    }

    /// <summary>
    /// Include soft-deleted entities in the results. Other query filters, like multi-tenancy, still apply.
    /// </summary>
    /// <param name="query">The query</param>
    /// <typeparam name="T">The type of entity being queried</typeparam>
    /// <returns>The query including soft-deleted entities</returns>
    public static IQueryable<T> IncludeSoftDeleted<T>(this IQueryable<T> query) where T : class, ISoftDeletable
    {
        return query.IgnoreQueryFilters([QueryFilterName]);
    }

    /// <summary>
    /// Only return soft-deleted entities, for example to list a recycle bin. Other query filters still apply.
    /// </summary>
    /// <param name="query">The query</param>
    /// <typeparam name="T">The type of entity being queried</typeparam>
    /// <returns>The query limited to soft-deleted entities</returns>
    public static IQueryable<T> OnlySoftDeleted<T>(this IQueryable<T> query) where T : class, ISoftDeletable
    {
        return query.IncludeSoftDeleted().Where(e => e.IsDeleted);
    }
}
