using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using VoidCore.Model.Auth;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;
using VoidCore.Test.EfIntegration.TestModels.Data;

namespace VoidCore.Test.EfIntegration.TestModels
{
    /// <summary>
    /// Test Dependencies
    /// </summary>
    public static class Deps
    {
        static Deps()
        {
            var userAccessorMock = new Mock<ICurrentUserAccessor>();
            userAccessorMock.Setup(a => a.User).Returns(new DomainUser("SingleUser", new string[0]));
            CurrentUserAccessor = userAccessorMock.Object;

            DateTimeServiceEarly = new DiscreteDateTimeService(new DateTime(2001, 1, 1, 11, 11, 11, DateTimeKind.Utc));

            DateTimeServiceLate = new DiscreteDateTimeService(new DateTime(2002, 2, 2, 22, 22, 22, DateTimeKind.Utc));
        }

        public static readonly IDateTimeService DateTimeServiceEarly;
        public static readonly IDateTimeService DateTimeServiceLate;
        public static readonly ICurrentUserAccessor CurrentUserAccessor;

        public static FoodStuffsContext FoodStuffsContext(string dbName = null)
        {
            return new FoodStuffsContext(
                new DbContextOptionsBuilder<FoodStuffsContext>()
                .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
                .Options
            );
        }

        public static FoodStuffsEfData FoodStuffsData(this FoodStuffsContext context)
        {
            var loggingStrategyMock = new Mock<ILoggingStrategy>();
            loggingStrategyMock.Setup(x => x.Log(It.IsAny<string[]>())).Returns("test request");
            return new FoodStuffsEfData(context, loggingStrategyMock.Object, DateTimeServiceLate, CurrentUserAccessor);
        }

        public static FoodStuffsContext Seed(this FoodStuffsContext data)
        {
            var category1 = data.Category.Add(new Category { Name = "Category1" }).Entity.Id;
            var category2 = data.Category.Add(new Category { Name = "Category2" }).Entity.Id;
            var category3 = data.Category.Add(new Category { Name = "Category3" }).Entity.Id;

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
                ModifiedBy = "12"
            }).Entity.Id;

            var recipe2 = data.Recipe.Add(new Recipe
            {
                Name = "Recipe2",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2,
                CreatedOn = DateTimeServiceEarly.Moment,
                ModifiedOn = DateTimeServiceLate.Moment,
                CreatedBy = "11",
                ModifiedBy = "11"
            }).Entity.Id;

            data.Recipe.Add(new Recipe
            {
                Name = "Recipe3",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2,
                CreatedOn = DateTimeServiceEarly.Moment,
                ModifiedOn = DateTimeServiceLate.Moment,
                CreatedBy = "11",
                ModifiedBy = "11"
            });

            data.CategoryRecipe.Add(new CategoryRecipe { RecipeId = recipe1, CategoryId = category1 });
            data.CategoryRecipe.Add(new CategoryRecipe { RecipeId = recipe1, CategoryId = category2 });
            data.CategoryRecipe.Add(new CategoryRecipe { RecipeId = recipe2, CategoryId = category3 });

            data.SaveChanges();
            return data;
        }
    }
}
