# VoidCore.EntityFramework

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.EntityFramework.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.EntityFramework)

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

In these cases, we can override the SaveChanges(bool) methods in the DBContext to set audit properties on all entities upon saving. It will also prevent soft-deletes from actually deleting entities. See the code below for an example.

You can still use the repository abstractions on top of this modified DBContext; just don't decorate your repositories using AddAuditability or AddSoftDeletability.

```csharp
public partial MyDbContext
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public MyDbContext(DbContextOptions<MyDbContext> options, IDateTimeService dateTimeService, ICurrentUserAccessor currentUserAccessor) : base(options)
    {
        _dateTimeService = dateTimeService;
        _currentUserAccessor = currentUserAccessor;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.Entries().SetAllAuditableProperties(_dateTimeService, (await _currentUserAccessor.GetUser()).Login);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.Entries().SetAllAuditableProperties(_dateTimeService, (await _currentUserAccessor.GetUser()).Login);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
```
