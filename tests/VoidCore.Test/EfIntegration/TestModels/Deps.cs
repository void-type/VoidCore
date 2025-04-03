using Microsoft.EntityFrameworkCore;
using NSubstitute;
using VoidCore.Model.Auth;
using VoidCore.Model.Time;
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
        CurrentUserAccessor = userAccessorMock;

        var early = new DateTime(2001, 1, 1, 11, 11, 11, DateTimeKind.Utc);
        var late = new DateTime(2002, 2, 2, 22, 22, 22, DateTimeKind.Utc);

        DateTimeServiceEarly = new DiscreteDateTimeService(early, new DateTimeOffset(early));
        DateTimeServiceLate = new DiscreteDateTimeService(late, new DateTimeOffset(late));
    }

    public static readonly IDateTimeService DateTimeServiceEarly;
    public static readonly IDateTimeService DateTimeServiceLate;
    public static readonly ICurrentUserAccessor CurrentUserAccessor;

    public static FoodStuffsContext FoodStuffsContext(string? dbName = null)
    {
        return new FoodStuffsContext(
            new DbContextOptionsBuilder<FoodStuffsContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options
        );
    }

    public static FoodStuffsContextAuditable FoodStuffsContextAuditable(string? dbName = null)
    {
        return new FoodStuffsContextAuditable(
            new DbContextOptionsBuilder<FoodStuffsContextAuditable>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options,
            DateTimeServiceLate,
            CurrentUserAccessor
        );
    }

    public static FoodStuffsEfData FoodStuffsData(this FoodStuffsContext context)
    {
        return new FoodStuffsEfData(context, DateTimeServiceLate, CurrentUserAccessor);
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
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
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
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
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
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
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
            CreatedOn = DateTimeServiceEarly.MomentWithOffset,
            ModifiedBy = "Void",
            ModifiedOn = DateTimeServiceEarly.MomentWithOffset
        }).Entity.Id;

        var category2 = data.Category.Add(new Category
        {
            Name = "Category2",
            CreatedBy = "Void",
            CreatedOn = DateTimeServiceEarly.MomentWithOffset,
            ModifiedBy = "Void",
            ModifiedOn = DateTimeServiceEarly.MomentWithOffset
        }).Entity.Id;

        var category3 = data.Category.Add(new Category
        {
            Name = "Category3",
            CreatedBy = "Void",
            CreatedOn = DateTimeServiceEarly.MomentWithOffset,
            ModifiedBy = "Void",
            ModifiedOn = DateTimeServiceEarly.MomentWithOffset
        }).Entity.Id;

        var recipe1 = data.Recipe.Add(new Recipe
        {
            Name = "Recipe1",
            Ingredients = "ing",
            Directions = "dir",
            CookTimeMinutes = 21,
            PrepTimeMinutes = 2,
            CreatedBy = "Void",
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedBy = "Void",
            ModifiedOn = DateTimeServiceEarly.Moment
        }).Entity.Id;

        var recipe2 = data.Recipe.Add(new Recipe
        {
            Name = "Recipe2",
            Directions = "",
            Ingredients = "",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedBy = "Void",
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedBy = "Void",
            ModifiedOn = DateTimeServiceEarly.Moment
        }).Entity.Id;

        data.Recipe.Add(new Recipe
        {
            Name = "Recipe3",
            Directions = "",
            Ingredients = "",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedBy = "Void",
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedBy = "Void",
            ModifiedOn = DateTimeServiceEarly.Moment
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

        data.SaveSeeding();
        return data;
    }
}
