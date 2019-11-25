using System.Linq;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Test.EfIntegration.TestModels;
using VoidCore.Test.EfIntegration.TestModels.Data;
using VoidCore.Test.EfIntegration.TestModels.Events;
using Xunit;

namespace VoidCore.Test.EfIntegration
{
    public class EfRepositoryTests
    {
        [Fact]
        public async Task Get_recipe_returns_recipe_by_id_when_found()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var recipes = await data.Recipes.ListAll(default);
            var recipeToFind = recipes.First();

            var byId = new RecipesByIdWithCategoriesSpecification(recipeToFind.Id);

            var recipe = await data.Recipes.Get(byId, default);

            Assert.True(recipe.HasValue);
            Assert.Equal(recipeToFind.Id, recipe.Value.Id);
        }

        [Fact]
        public async Task Get_recipe_returns_failure_when_recipe_not_found()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var byId = new RecipesByIdWithCategoriesSpecification(-22);

            var recipe = await data.Recipes.Get(byId, default);

            Assert.True(recipe.HasNoValue);
        }

        [Fact]
        public async Task ListRecipes_returns_a_page_of_recipes()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, null, null, true, 2, 1));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(2, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
        }

        [Fact]
        public async Task ListRecipe_returns_all_recipes_when_paging_is_disabled()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, null, null, false, 0, 0));

            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Contains("Recipe1", result.Value.Items.Select(r => r.Name));
            Assert.Contains("Recipe2", result.Value.Items.Select(r => r.Name));
            Assert.Contains("Recipe3", result.Value.Items.Select(r => r.Name));
            Assert.Contains("Category1", result.Value.Items.SelectMany(r => r.Categories));
        }

        [Fact]
        public async Task ListRecipes_can_sort_by_descending()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, null, "nameDesc", true, 1, 1));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
            Assert.Equal("Recipe3", result.Value.Items.First().Name);
        }

        [Fact]
        public async Task ListRecipes_can_sort_by_ascending()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, null, "name", true, 1, 1));

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(3, result.Value.TotalCount);
            Assert.Equal(1, result.Value.Page);
            Assert.Equal(1, result.Value.Take);
            Assert.Contains("Recipe1", result.Value.Items.First().Name);
        }

        [Fact]
        public async Task ListRecipes_can_search_by_recipe_name()
        {
            using var context = Deps.FoodStuffsContext();
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

        [Fact]
        public async Task ListRecipes_can_search_by_category_name()
        {
            using var context = Deps.FoodStuffsContext();
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

        [Fact]
        public async Task ListRecipes_returns_empty_item_set_when_name_search_matches_zero_items()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request("nothing matches", null, null, true, 1, 2));

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Count);
            Assert.Equal(0, result.Value.TotalCount);
        }

        [Fact]
        public async Task ListRecipes_returns_empty_item_set_when_category_search_matches_zero_items()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, "nothing matches", null, true, 1, 2));

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Count);
            Assert.Equal(0, result.Value.TotalCount);
        }

        [Fact]
        public async Task ListRecipes_returns_empty_list_when_none_found_by_name_search()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request("nothing matches", null, null, true, 1, 2));

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Count);
            Assert.Equal(0, result.Value.TotalCount);
        }

        [Fact]
        public async Task ListRecipes_returns_empty_list_when_none_found_by_category_search()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new ListRecipes.Handler(data)
                .Handle(new ListRecipes.Request(null, "nothing matches", null, true, 1, 2));

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value.Count);
            Assert.Equal(0, result.Value.TotalCount);
        }

        [Fact]
        public async Task DeleteRecipe_deletes_recipe_if_found()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var recipes = await data.Recipes.ListAll(default);
            var recipeToDelete = recipes.First();

            var byId = new RecipesByIdWithCategoriesSpecification(recipeToDelete.Id);

            var recipe = await data.Recipes.Get(byId, default).UnwrapAsync();

            await data.CategoryRecipes.RemoveRange(recipe.CategoryRecipe.AsReadOnly(), default);
            await data.Recipes.Remove(recipe, default);

            var recipeMaybe = await data.Recipes.Get(byId, default);

            Assert.True(recipeMaybe.HasNoValue);
        }

        [Fact]
        public async Task SaveRecipe_creates_new_recipe_when_id_0_is_specified()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var result = await new SaveRecipe.Handler(data)
                .Handle(new SaveRecipe.Request(0, "New", "New", "New", null, 20, new[] { "Category2", "Category3", "Category4" }));

            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Id > 0);

            var maybeRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(result.Value.Id), default);

            Assert.True(maybeRecipe.HasValue);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, maybeRecipe.Value.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, maybeRecipe.Value.ModifiedOn);
            Assert.DoesNotContain("Category1", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category2", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category3", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category4", maybeRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
        }

        [Fact]
        public async Task SaveRecipe_updates_existing_recipe_when_exists()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var existingRecipeId = (await data.Recipes.ListAll(default)).First().Id;

            var result = await new SaveRecipe.Handler(data)
                .Handle(new SaveRecipe.Request(existingRecipeId, "New", "New", "New", null, 20, new[] { "Category2", "Category3", "Category4" }));

            Assert.True(result.IsSuccess);
            Assert.Equal(existingRecipeId, result.Value.Id);

            var updatedRecipe = await data.Recipes.Get(new RecipesByIdWithCategoriesSpecification(existingRecipeId), default);
            Assert.True(updatedRecipe.HasValue);
            Assert.Equal(Deps.DateTimeServiceEarly.Moment, updatedRecipe.Value.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, updatedRecipe.Value.ModifiedOn);
            Assert.DoesNotContain("Category1", updatedRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category2", updatedRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category3", updatedRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
            Assert.Contains("Category4", updatedRecipe.Value.CategoryRecipe.Select(cr => cr.Category.Name));
        }

        [Fact]
        public async Task Range_of_recipes_can_be_updated()
        {
            using var context = Deps.FoodStuffsContext();
            var data = context.Seed().FoodStuffsData();

            var recipes = await data.Recipes.ListAll(default);

            foreach (var recipe in recipes)
            {
                recipe.Name = "changeMe";
            }

            await data.Recipes.UpdateRange(recipes, default);

            var updatedRecipes = await data.Recipes.ListAll(default)
                .MapAsync(recs => recs.Where(r => r.Name == "changeMe").ToList());

            Assert.Equal(3, updatedRecipes.Count);
        }
    }
}
