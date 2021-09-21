using System.Linq;
using System.Threading.Tasks;
using VoidCore.Test.EfIntegration.TestModels;
using VoidCore.Test.EfIntegration.TestModels.Data;
using Xunit;

namespace VoidCore.Test.EfIntegration
{
    public class EfAuditableDbContextTests
    {
        [Fact]
        public async Task Add_sets_creation_properties()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var r = new Recipe() { Name = "new recipe" };
            context.Recipe.Add(r);
            await context.SaveChangesAsync();

            Assert.Equal(Deps.CurrentUserAccessor.User.Login, r.CreatedBy);
            Assert.Equal(Deps.CurrentUserAccessor.User.Login, r.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceLate.Moment, r.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, r.ModifiedOn);
        }

        [Fact]
        public async Task Add_sets_creation_properties_offset()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var c = new Category() { Name = "new category" };
            context.Category.Add(c);
            await context.SaveChangesAsync();

            Assert.Equal(Deps.CurrentUserAccessor.User.Login, c.CreatedBy);
            Assert.Equal(Deps.CurrentUserAccessor.User.Login, c.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceLate.MomentWithOffset, c.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.MomentWithOffset, c.ModifiedOn);
        }

        [Fact]
        public async Task Modify_sets_modification_properties()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var r = context.Recipe.First();

            r.Name = "new name";
            await context.SaveChangesAsync();

            Assert.Equal("Void", r.CreatedBy);
            Assert.Equal(Deps.CurrentUserAccessor.User.Login, r.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.Moment, r.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, r.ModifiedOn);
        }

        [Fact]
        public async Task Modify_sets_modification_properties_offset()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var c = context.Category.First();

            c.Name = "new name";
            await context.SaveChangesAsync();

            Assert.Equal("Void", c.CreatedBy);
            Assert.Equal(Deps.CurrentUserAccessor.User.Login, c.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, c.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceLate.MomentWithOffset, c.ModifiedOn);
        }

        [Fact]
        public async Task Delete_sets_soft_delete_properties()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var r = context.Recipe.First();

            context.CategoryRecipe.RemoveRange(context.CategoryRecipe.Where(x => x.Recipe == r).ToList());
            context.Recipe.Remove(r);
            await context.SaveChangesAsync();

            Assert.Equal("Void", r.CreatedBy);
            Assert.Equal("Void", r.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.Moment, r.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceEarly.Moment, r.ModifiedOn);

            Assert.Equal(Deps.CurrentUserAccessor.User.Login, r.DeletedBy);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, r.DeletedOn);
            Assert.True(r.IsDeleted);
        }

        [Fact]
        public async Task Delete_sets_soft_delete_properties_offset()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var c = context.Category.First();

            context.CategoryRecipe.RemoveRange(context.CategoryRecipe.Where(x => x.Category == c).ToList());
            context.Remove(c);
            await context.SaveChangesAsync();

            Assert.Equal("Void", c.CreatedBy);
            Assert.Equal("Void", c.ModifiedBy);

            Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, c.CreatedOn);
            Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, c.ModifiedOn);

            Assert.Equal(Deps.CurrentUserAccessor.User.Login, c.DeletedBy);
            Assert.Equal(Deps.DateTimeServiceLate.MomentWithOffset, c.DeletedOn);
            Assert.True(c.IsDeleted);
        }

        [Fact]
        public async Task Saving_doesnt_throw_exception_on_ignored_entities()
        {
            using var context = Deps.FoodStuffsContextAuditable().Seed();

            var cr = context.CategoryRecipe.First();

            cr.Name = "new name";
            await context.SaveChangesAsync();
        }
    }
}
