using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using VoidCore.Model.Auth;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;
using VoidCore.Test.AspNet.Data.TestModels.Data;

namespace VoidCore.Test.AspNet.Data.TestModels
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
            data.Category.Add(new Category { Id = 11, Name = "Category1" });
            data.Category.Add(new Category { Id = 12, Name = "Category2" });
            data.Category.Add(new Category { Id = 13, Name = "Category3" });

            data.Recipe.Add(new Recipe
            {
                Id = 11,
                Name = "Recipe1",
                Ingredients = "ing",
                Directions = "dir",
                CookTimeMinutes = 21,
                PrepTimeMinutes = 2,
                CreatedOn = DateTimeServiceEarly.Moment,
                ModifiedOn = DateTimeServiceLate.Moment,
                CreatedBy = "11",
                ModifiedBy = "12"
            });

            data.Recipe.Add(new Recipe
            {
                Id = 12,
                Name = "Recipe2",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2,
                CreatedOn = DateTimeServiceEarly.Moment,
                ModifiedOn = DateTimeServiceLate.Moment,
                CreatedBy = "11",
                ModifiedBy = "11"
            });

            data.Recipe.Add(new Recipe
            {
                Id = 13,
                Name = "Recipe3",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2,
                CreatedOn = DateTimeServiceEarly.Moment,
                ModifiedOn = DateTimeServiceLate.Moment,
                CreatedBy = "11",
                ModifiedBy = "11"
            });

            data.CategoryRecipe.Add(new CategoryRecipe { RecipeId = 11, CategoryId = 11 });
            data.CategoryRecipe.Add(new CategoryRecipe { RecipeId = 11, CategoryId = 12 });
            data.CategoryRecipe.Add(new CategoryRecipe { RecipeId = 12, CategoryId = 11 });

            data.SaveChanges();
            return data;
        }
    }
}
