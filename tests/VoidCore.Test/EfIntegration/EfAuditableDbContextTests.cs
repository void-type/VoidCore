using System.Linq;
using System.Threading.Tasks;
using VoidCore.Test.EfIntegration.TestModels;
using Xunit;

namespace VoidCore.Test.EfIntegration
{
    public class EfAuditableDbContextTests
    {
        [Fact]
        public void Check_add_sets_creation_properties()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var recipe = context.Recipe.First();

            Assert.Equal("Void", recipe.CreatedBy);
            Assert.Equal("Void", recipe.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.Moment, recipe.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceEarly.Moment, recipe.ModifiedOn);
        }

        [Fact]
        public void Check_add_sets_creation_properties_offset()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var category = context.Category.First();

            Assert.Equal("Void", category.CreatedBy);
            Assert.Equal("Void", category.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, category.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, category.ModifiedOn);
        }

        [Fact]
        public async Task Check_modify_sets_modification_properties()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var category = context.Category.First();

            category.Name = "new name";
            await context.SaveChangesAsync();

            Assert.Equal("Void", category.CreatedBy);
            Assert.Equal(Deps.CurrentUserAccessor.User.Login, category.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.Moment, category.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, category.ModifiedOn);
        }

        [Fact]
        public async Task Check_modify_sets_modification_properties_offset()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var category = context.Category.First();

            category.Name = "new name";
            await context.SaveChangesAsync();

            Assert.Equal("Void", category.CreatedBy);
            Assert.Equal(Deps.CurrentUserAccessor.User.Login, category.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, category.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.MomentWithOffset, category.ModifiedOn);
        }

        [Fact]
        public async Task Check_delete_sets_soft_delete_properties()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var recipe = context.Recipe.First();
            var categoryRecipes = context.CategoryRecipe.Where(x => x.Recipe == recipe);

            context.CategoryRecipe.RemoveRange(categoryRecipes);

            context.Recipe.Remove(recipe);
            await context.SaveChangesAsync();

            Assert.Equal("Void", recipe.CreatedBy);
            Assert.Equal("Void", recipe.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.Moment, recipe.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceEarly.Moment, recipe.ModifiedOn);

            Assert.Equal(Deps.CurrentUserAccessor.User.Login, recipe.DeletedBy);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, recipe.DeletedOn);
        }

        [Fact]
        public async Task Check_delete_sets_soft_delete_properties_offset()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var categoryRecipe = context.CategoryRecipe.First();

            context.Remove(categoryRecipe);
            await context.SaveChangesAsync();

            Assert.Equal("Void", categoryRecipe.CreatedBy);
            Assert.Equal("Void", categoryRecipe.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, categoryRecipe.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, categoryRecipe.ModifiedOn);

            Assert.Equal(Deps.CurrentUserAccessor.User.Login, categoryRecipe.DeletedBy);
            Assert.Equal(Deps.DateTimeServiceLate.MomentWithOffset, categoryRecipe.DeletedOn);
        }
    }
}
