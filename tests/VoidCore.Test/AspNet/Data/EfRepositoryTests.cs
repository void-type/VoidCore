using Moq;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Model.Auth;
using VoidCore.Test.AspNet.Data.TestModels;
using VoidCore.Test.AspNet.Data.TestModels.Data;
using VoidCore.Test.AspNet.Data.TestModels.Events;
using Xunit;

namespace VoidCore.Test.AspNet.Data
{
    public class EfRepositoryTests
    {
        [Fact]
        public async Task GetRecipeFound()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var byId = new RecipesByIdWithCategoriesSpecification(11);

                var recipe = await data.Recipes.Get(byId);

                Assert.True(recipe.HasValue);
                Assert.Equal(11, recipe.Value.Id);
            }
        }

        [Fact]
        public async Task GetRecipeNotFound()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var byId = new RecipesByIdWithCategoriesSpecification(1000);

                var recipe = await data.Recipes.Get(byId);

                Assert.True(recipe.HasNoValue);
            }
        }

        [Fact]
        public async Task ListAllRecipes()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var recipes = await data.Recipes.ListAll();

                Assert.Equal(3, recipes.Count);
                Assert.Contains("Recipe2", recipes.Select(r => r.Name));
                Assert.Contains(12, recipes.Select(r => r.Id));
                Assert.Contains("Category1", recipes.SelectMany(r => r.CategoryRecipe.Select(cr => cr.Category.Name)));
            }
        }

        [Fact]
        public async Task ListRecipesWithoutPaging()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request(null, null, null, false, 0, 0));

                Assert.True(result.IsSuccess);
                Assert.Equal(3, result.Value.Count);
                Assert.Equal(3, result.Value.TotalCount);
                Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
                Assert.Contains(12, result.Value.Items.Select(r => r.Id));
                Assert.Contains("Category1", result.Value.Items.SelectMany(r => r.Categories));
            }
        }

        [Fact]
        public async Task ListRecipesSortDesc()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request(null, null, "nameDesc", true, 1, 1));

                Assert.True(result.IsSuccess);
                Assert.Equal(1, result.Value.Count);
                Assert.Equal(3, result.Value.TotalCount);
                Assert.Equal(1, result.Value.Page);
                Assert.Equal(1, result.Value.Take);
                Assert.Contains("Recipe3", result.Value.Items.Select(r => r.Name));
            }
        }

        [Fact]
        public async Task ListRecipesSortAscend()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                await data.Recipes.Add(new Recipe { Name = "ANewRecipe" });

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request(null, null, "name", true, 1, 1));

                Assert.True(result.IsSuccess);
                Assert.Equal(1, result.Value.Count);
                Assert.Equal(4, result.Value.TotalCount);
                Assert.Equal(1, result.Value.Page);
                Assert.Equal(1, result.Value.Take);
                Assert.Contains("ANewRecipe", result.Value.Items.Select(r => r.Name));
            }
        }

        [Fact]
        public async Task ListRecipesNameSearch()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request("recipe2", null, null, true, 1, 2));

                Assert.True(result.IsSuccess);
                Assert.Equal(1, result.Value.Count);
                Assert.Equal(1, result.Value.TotalCount);
                Assert.Equal(1, result.Value.Page);
                Assert.Equal(2, result.Value.Take);
                Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
            }
        }

        [Fact]
        public async Task ListRecipesCategorySearch()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request(null, "cat", null, true, 1, 4));

                Assert.True(result.IsSuccess);
                Assert.Equal(2, result.Value.Count);
                Assert.Equal(2, result.Value.TotalCount);
                Assert.Equal(1, result.Value.Page);
                Assert.Equal(4, result.Value.Take);
                Assert.Contains("Recipe1", result.Value.Items.Select(r => r.Name));
                Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
                Assert.DoesNotContain("Recipe3", result.Value.Items.Select(r => r.Name));
            }
        }

        [Fact]
        public async Task ListRecipesNoneFoundByNameSearch()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request("nothing matches", null, null, true, 1, 2));

                Assert.True(result.IsSuccess);
                Assert.Equal(0, result.Value.Count);
                Assert.Equal(0, result.Value.TotalCount);
            }
        }

        [Fact]
        public async Task ListRecipesNoneFoundByCategorySearch()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var result = await new ListRecipes.Handler(data)
                    .Handle(new ListRecipes.Request(null, "nothing matches", null, true, 1, 2));

                Assert.True(result.IsSuccess);
                Assert.Equal(0, result.Value.Count);
                Assert.Equal(0, result.Value.TotalCount);
            }
        }

        [Fact]
        public async Task DeleteRecipeFound()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var byId = new RecipesByIdWithCategoriesSpecification(11);

                var recipe = await data.Recipes.Get(byId).UnwrapAsync();

                await data.CategoryRecipes.RemoveRange(recipe.CategoryRecipe.AsReadOnly());
                await data.Recipes.Remove(recipe);

                var recipeMaybe = await data.Recipes.Get(byId);

                Assert.True(recipeMaybe.HasNoValue);
            }
        }

        [Fact]
        public async Task SaveNewRecipe()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var user = new DomainUser("SingleUser", new string[0]);

                var userAccessorMock = new Mock<ICurrentUserAccessor>();
                userAccessorMock.Setup(a => a.User).Returns(user);

                var result = await new SaveRecipe.Handler(data)
                    .Handle(new SaveRecipe.Request(0, "New", "New", "New", null, 20, new[] { "Category2", "Category3", "Category4" }));

                Assert.True(result.IsSuccess);
                Assert.True(result.Value.Id > 0);

                var maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(result.Value.Id));
                Assert.True(maybeRecipe.HasValue);
                Assert.Equal(Deps.DateTimeServiceLate.Moment, maybeRecipe.Value.CreatedOn);
                Assert.Equal(Deps.DateTimeServiceLate.Moment, maybeRecipe.Value.ModifiedOn);
                Assert.DoesNotContain("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.Contains("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.Contains("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.Contains("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));

                maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(12));
                Assert.True(maybeRecipe.HasValue);
                Assert.Contains("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.DoesNotContain("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.DoesNotContain("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.DoesNotContain("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            }
        }

        [Fact]
        public async Task SaveExistingRecipe()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var user = new DomainUser("SingleUser", new string[0]);

                var userAccessorMock = new Mock<ICurrentUserAccessor>();
                userAccessorMock.Setup(a => a.User).Returns(user);

                var maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(11));
                Assert.True(maybeRecipe.HasValue);

                var result = await new SaveRecipe.Handler(data)
                    .Handle(new SaveRecipe.Request(11, "New", "New", "New", null, 20, new[] { "Category2", "Category3", "Category4" }));

                Assert.True(result.IsSuccess);
                Assert.Equal(11, result.Value.Id);

                maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(11));
                Assert.True(maybeRecipe.HasValue);
                Assert.Equal(Deps.DateTimeServiceEarly.Moment, maybeRecipe.Value.CreatedOn);
                Assert.Equal(Deps.DateTimeServiceLate.Moment, maybeRecipe.Value.ModifiedOn);
                Assert.DoesNotContain("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.Contains("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.Contains("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.Contains("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));

                maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(12));
                Assert.True(maybeRecipe.HasValue);
                Assert.Contains("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.DoesNotContain("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.DoesNotContain("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
                Assert.DoesNotContain("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            }
        }

        [Fact]
        public async Task UpdateRangeOfRecipes()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.Seed().FoodStuffsData();

                var recipes = await data.Recipes.ListAll();

                foreach (var recipe in recipes)
                {
                    recipe.Name = "changeMe";
                }

                await data.Recipes.UpdateRange(recipes);

                var updatedRecipes = await data.Recipes.ListAll()
                    .MapAsync(recs => recs.Where(r => r.Name == "changeMe").ToList());

                Assert.Equal(3, updatedRecipes.Count);
                Assert.Contains(11, recipes.Select(r => r.Id));
                Assert.Contains(12, recipes.Select(r => r.Id));
                Assert.Contains(13, recipes.Select(r => r.Id));
            }
        }

        [Fact]
        public async Task QueryViewWithSpecification()
        {
            using (var context = Deps.FoodStuffsContext())
            {
                var data = context.FoodStuffsData();

                var users1 = await data.Users.List(new UserSpecification(u => u.JoinedOn < Deps.DateTimeServiceLate.Moment));

                Assert.Equal(2, users1.Count);

                var users2 = await data.Users.ListAll();

                Assert.Equal(3, users2.Count);
            }
        }
    }
}
