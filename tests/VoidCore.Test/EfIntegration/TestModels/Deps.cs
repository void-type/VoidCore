using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using VoidCore.EntityFramework;
using VoidCore.Model.Auth;
using VoidCore.Test.EfIntegration.TestModels.Data;

namespace VoidCore.Test.EfIntegration.TestModels;

/// <summary>
/// Test Dependencies
/// </summary>
public static class Deps
{
    static Deps()
    {
        var userAccessorMock = Substitute.For<ICurrentUserAccessor>();
        userAccessorMock.GetUser().Returns(new DomainUser("SingleUser", Array.Empty<string>()));
        userAccessorMock.GetUserName().Returns("SingleUser");
        CurrentUserAccessor = userAccessorMock;

        Early = new DateTimeOffset(2001, 1, 1, 11, 11, 11, TimeSpan.Zero);
        Late = new DateTimeOffset(2002, 2, 2, 22, 22, 22, TimeSpan.Zero);

        TimeProviderEarly = new FakeTimeProvider(Early);
        TimeProviderLate = new FakeTimeProvider(Late);

        var auditableInterceptor = new AuditableSaveChangesInterceptor(TimeProviderLate, CurrentUserAccessor);
        var softDeletableInterceptor = new SoftDeletableSaveChangesInterceptor(TimeProviderLate, CurrentUserAccessor);
        var requireRecycleBinInterceptor = new SoftDeletableSaveChangesInterceptor(TimeProviderLate, CurrentUserAccessor) { RequireRecycleBin = true };

        s_interceptors = new()
        {
            [(true, false)] = [softDeletableInterceptor, auditableInterceptor],
            [(false, false)] = [auditableInterceptor, softDeletableInterceptor],
            [(true, true)] = [requireRecycleBinInterceptor, auditableInterceptor],
            [(false, true)] = [auditableInterceptor, requireRecycleBinInterceptor],
        };
    }

    public static readonly DateTimeOffset Early;
    public static readonly DateTimeOffset Late;
    public static readonly TimeProvider TimeProviderEarly;
    public static readonly TimeProvider TimeProviderLate;
    public static readonly ICurrentUserAccessor CurrentUserAccessor;

    // Interceptors are shared so EF can reuse its internal service provider across contexts.
    private static readonly Dictionary<(bool SoftDeletableFirst, bool RequireRecycleBin), IInterceptor[]> s_interceptors;
    private static readonly InMemoryDatabaseRoot s_auditableDatabaseRoot = new();

    public static FoodStuffsContext FoodStuffsContext(string? dbName = null)
    {
        return new FoodStuffsContext(
            new DbContextOptionsBuilder<FoodStuffsContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options
        );
    }

    /// <summary>
    /// A context with the auditable and soft-deletable interceptors. Seed data is saved through a separate context without them so the seeded values aren't overwritten.
    /// </summary>
    /// <param name="dbName">The name of the in-memory database</param>
    /// <param name="softDeletableFirst">The order the interceptors run in. Both orders should give the same results.</param>
    /// <param name="requireRecycleBin">Make deleting an entity that hasn't been soft-deleted throw</param>
    public static FoodStuffsContextAuditable FoodStuffsContextAuditable(string? dbName = null, bool softDeletableFirst = true, bool requireRecycleBin = false)
    {
        dbName ??= Guid.NewGuid().ToString();

        using (var seedingContext = new FoodStuffsContextAuditable(AuditableOptions(dbName, [])))
        {
            seedingContext.Seed();
        }

        return new FoodStuffsContextAuditable(AuditableOptions(dbName, s_interceptors[(softDeletableFirst, requireRecycleBin)]));
    }

    private static DbContextOptions<FoodStuffsContextAuditable> AuditableOptions(string dbName, IInterceptor[] interceptors)
    {
        // The interceptors change EF's internal service provider, so the in-memory store must be shared explicitly.
        return new DbContextOptionsBuilder<FoodStuffsContextAuditable>()
            .UseInMemoryDatabase(dbName, s_auditableDatabaseRoot)
            .AddInterceptors(interceptors)
            .Options;
    }

    public static FoodStuffsEfData FoodStuffsData(this FoodStuffsContext context)
    {
        return new FoodStuffsEfData(context, TimeProviderLate, CurrentUserAccessor);
    }

    public static FoodStuffsContext Seed(this FoodStuffsContext data)
    {
        var category1 = data.Category.Add(new Category
        {
            Name = "Category1",
            CreatedBy = "11",
            DeletedBy = "",
            ModifiedBy = "33",
        }).Entity.Id;

        var category2 = data.Category.Add(new Category
        {
            Name = "Category2",
            CreatedBy = "11",
            DeletedBy = "",
            ModifiedBy = "33",
        }).Entity.Id;

        var category3 = data.Category.Add(new Category
        {
            Name = "Category3",
            CreatedBy = "11",
            DeletedBy = "",
            ModifiedBy = "33",
        }).Entity.Id;

        var recipe1 = data.Recipe.Add(new Recipe
        {
            Name = "Recipe1",
            Ingredients = "ing",
            Directions = "dir",
            CookTimeMinutes = 21,
            PrepTimeMinutes = 2,
            CreatedOn = Early,
            ModifiedOn = Late,
            CreatedBy = "11",
            ModifiedBy = "12",
            DeletedBy = "",
        }).Entity.Id;

        var recipe2 = data.Recipe.Add(new Recipe
        {
            Name = "Recipe2",
            Directions = "",
            Ingredients = "",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = Early,
            ModifiedOn = Late,
            CreatedBy = "11",
            ModifiedBy = "11",
            DeletedBy = "",
        }).Entity.Id;

        data.Recipe.Add(new Recipe
        {
            Name = "Recipe3",
            Directions = "",
            Ingredients = "",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = Early,
            ModifiedOn = Late,
            CreatedBy = "11",
            ModifiedBy = "11",
            DeletedBy = "",
        });

        data.CategoryRecipe.Add(new CategoryRecipe
        {
            RecipeId = recipe1,
            CategoryId = category1,
        });

        data.CategoryRecipe.Add(new CategoryRecipe
        {
            RecipeId = recipe1,
            CategoryId = category2
        });

        data.CategoryRecipe.Add(new CategoryRecipe
        {
            RecipeId = recipe2,
            CategoryId = category3
        });

        data.SaveChanges();
        return data;
    }

    public static FoodStuffsContextAuditable Seed(this FoodStuffsContextAuditable data)
    {
        var category1 = data.Category.Add(new Category
        {
            Name = "Category1",
            CreatedBy = "Void",
            CreatedOn = Early,
            ModifiedBy = "Void",
            ModifiedOn = Early
        }).Entity.Id;

        var category2 = data.Category.Add(new Category
        {
            Name = "Category2",
            CreatedBy = "Void",
            CreatedOn = Early,
            ModifiedBy = "Void",
            ModifiedOn = Early
        }).Entity.Id;

        var category3 = data.Category.Add(new Category
        {
            Name = "Category3",
            CreatedBy = "Void",
            CreatedOn = Early,
            ModifiedBy = "Void",
            ModifiedOn = Early
        }).Entity.Id;

        var recipe1 = data.Recipe.Add(new Recipe
        {
            Name = "Recipe1",
            Ingredients = "ing",
            Directions = "dir",
            CookTimeMinutes = 21,
            PrepTimeMinutes = 2,
            CreatedBy = "Void",
            CreatedOn = Early,
            ModifiedBy = "Void",
            ModifiedOn = Early
        }).Entity.Id;

        var recipe2 = data.Recipe.Add(new Recipe
        {
            Name = "Recipe2",
            Directions = "",
            Ingredients = "",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedBy = "Void",
            CreatedOn = Early,
            ModifiedBy = "Void",
            ModifiedOn = Early
        }).Entity.Id;

        data.Recipe.Add(new Recipe
        {
            Name = "Recipe3",
            Directions = "",
            Ingredients = "",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedBy = "Void",
            CreatedOn = Early,
            ModifiedBy = "Void",
            ModifiedOn = Early
        });

        data.CategoryRecipe.Add(new CategoryRecipe
        {
            RecipeId = recipe1,
            CategoryId = category1
        });

        data.CategoryRecipe.Add(new CategoryRecipe
        {
            RecipeId = recipe1,
            CategoryId = category2
        });

        data.CategoryRecipe.Add(new CategoryRecipe
        {
            RecipeId = recipe2,
            CategoryId = category3
        });

        data.SaveChanges();
        return data;
    }
}
