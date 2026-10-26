# VoidCore.EntityFramework

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/)

## Installation

```powerShell
dotnet add package VoidCore.EntityFramework
```

## Features

Add an Entity Framework data layer to your application.

### Async Database Abstractions

Included are EntityFramework implementations of the specification and repository patterns from VoidCore.Model. These implementations defer the heavy lifting of searching, sorting, paging, and joining to the SQL Server. All calls are asynchronous and immediate.

Queries are automatically tagged with the repository and specification names they were called with. This helps tie SQL logs backs to the application code. See [Query Tags on Microsoft docs](https://docs.microsoft.com/en-us/ef/core/querying/tags) for more information.

See [VoidCore.Model](model.md) for more about it's data persistence features. You can decorate these EF repositories with Model's auditable and soft-delete decorators.

### Specification pattern

Encapsulate repeated queries within a specification object.

```csharp
public class RecipesSearchSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria, PaginationOptions paginationOptions, string sort = null, bool sortDesc = false) : base(criteria)
    {
        AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");

        ApplyPaging(paginationOptions);

        switch (sort)
        {
            case "name":
                AddOrderBy(recipe => recipe.Name, sortDesc);
                AddOrderBy(recipe => recipe.CreatedOn);
                break;

            default:
                AddOrderBy(recipe => recipe.Id);
                break;
        }
    }
}
```

Specifications can be used with the repositories above, or applied to an IQueryable.

```csharp
var spicyRecipes = await _dbContext.Recipes
    .ApplyEfSpecification(spicyRecipesSpecification)
    .ToListAsync();
```

### Audit and Soft-Delete capabilities when directly using DBContext

Sometimes specification repositories aren't a good fit for a project. Sometimes we need data services to hide complexity, or we simply just want to use the DBContext directly in our code.

In these cases, add interceptors to your DbContext. Each works for both SaveChanges and SaveChangesAsync with the same code path. Times are recorded in UTC using a `TimeProvider`, and the user name comes from `ICurrentUserAccessor.GetUserName()`.

* `AuditableSaveChangesInterceptor` sets audit properties on added and modified `IAuditable` entities.
* `SoftDeletableSaveChangesInterceptor` sets soft-delete properties on `ISoftDeletable` entities. Set `IsDeleted = true` and it records `DeletedOn` and `DeletedBy`. Set `IsDeleted = false` to restore the entity and they are cleared. Soft-deleting modifies the entity, so the audit interceptor records `ModifiedOn` and `ModifiedBy` too, in either order.

You can still use the repository abstractions on top of this DBContext; just don't decorate your repositories using AddAuditability or AddSoftDeletability. Those decorators remain available for non-EF repositories.

```csharp
// The interceptors are singletons, so ICurrentUserAccessor and TimeProvider must be singletons too.
services.AddSingleton(TimeProvider.System);
services.AddSingleton<AuditableSaveChangesInterceptor>();
services.AddSingleton<SoftDeletableSaveChangesInterceptor>();

services.AddDbContext<MyDbContext>((provider, options) => options
    .UseSqlServer(connectionString)
    .AddInterceptors(
        provider.GetRequiredService<AuditableSaveChangesInterceptor>(),
        provider.GetRequiredService<SoftDeletableSaveChangesInterceptor>()));
```

Your DbContext doesn't need any overrides or extra constructor parameters.

#### Querying soft-deleted entities

Soft-deleted rows stay in the database. To hide them from queries, call `ApplySoftDeleteQueryFilter()` last in `OnModelCreating`. It adds a named `SoftDelete` query filter (`!IsDeleted`) to every `ISoftDeletable` entity type. EF Core doesn't allow mixing named and unnamed filters on one entity, so other filters on those entities must be named, for example `HasQueryFilter("Tenant", ...)`.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
    modelBuilder.ApplySoftDeleteQueryFilter();
}
```

For recycle-bin operations, bypass only the soft-delete filter. Other filters, like multi-tenancy, still apply.

```csharp
// Soft-delete
recipe.IsDeleted = true;
await context.SaveChangesAsync();

// List the recycle bin
var deleted = await context.Recipes.OnlySoftDeleted().ToListAsync();

// Include deleted rows in a query
var all = await context.Recipes.IncludeSoftDeleted().ToListAsync();

// Restore
deleted[0].IsDeleted = false;
await context.SaveChangesAsync();

// Empty the recycle bin. The entities are loaded, so your handlers can still audit or clean up side effects first.
context.Recipes.RemoveRange(deleted.Skip(1));
await context.SaveChangesAsync();
```

Existing `DeletedOn` and `DeletedBy` values are never overwritten, so modifying a soft-deleted entity doesn't change when it was deleted. The stamps are also applied to disconnected entities saved with `context.Update`. Soft-deleting doesn't touch an entity's children, so flag dependents too if they should go to the bin together. EF Core warns when a required navigation points at a filtered entity. `ExecuteUpdate` and `ExecuteDelete` bypass interceptors.

#### Requiring the recycle bin

`Remove` is a normal, permanent delete, so a forgotten soft-delete loses data silently. To make the recycle bin mandatory, set `RequireRecycleBin`. Saving then throws if an `ISoftDeletable` entity is deleted that wasn't already soft-deleted in the database. Soft-delete and save first, then `Remove` to permanently delete. Setting `IsDeleted = true` and calling `Remove` in the same save still throws. Entities that aren't `ISoftDeletable` are unaffected.

```csharp
services.AddSingleton(provider => new SoftDeletableSaveChangesInterceptor(
    provider.GetRequiredService<TimeProvider>(),
    provider.GetRequiredService<ICurrentUserAccessor>())
{
    RequireRecycleBin = true,
});
```
