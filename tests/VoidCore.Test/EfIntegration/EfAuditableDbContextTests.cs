using VoidCore.Model.Data;
using VoidCore.Test.EfIntegration.TestModels;
using VoidCore.Test.EfIntegration.TestModels.Data;
using Xunit;

namespace VoidCore.Test.EfIntegration;

public class EfAuditableDbContextTests
{
    [Fact]
    public async Task Add_sets_creation_properties()
    {
        using var context = Deps.FoodStuffsContextAuditable().Seed();

        var r = new Recipe() { Name = "new recipe", Directions = "", Ingredients = "" };
        context.Recipe.Add(r);
        await context.SaveChangesAsync();

        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, r.CreatedBy);
        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, r.ModifiedBy);

        Assert.Equal(Deps.DateTimeServiceLate.Moment, r.CreatedOn);
        Assert.Equal(Deps.DateTimeServiceLate.Moment, r.ModifiedOn);
    }

    [Fact]
    public async Task Add_sets_creation_properties_offset()
    {
        using var context = Deps.FoodStuffsContextAuditable().Seed();

        var c = new Category()
        {
            Name = "new category",
            CreatedBy = "11",
            DeletedBy = "",
            ModifiedBy = "33",
        };
        context.Category.Add(c);
        await context.SaveChangesAsync();

        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, c.CreatedBy);
        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, c.ModifiedBy);

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
        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, r.ModifiedBy);

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
        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, c.ModifiedBy);

        Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, c.CreatedOn);
        Assert.Equal(Deps.DateTimeServiceLate.MomentWithOffset, c.ModifiedOn);
    }

    [Fact]
    public async Task Delete_sets_soft_delete_properties()
    {
        using var context = Deps.FoodStuffsContextAuditable().Seed();

        var r = context.Recipe.First();

        var toDelete = context.CategoryRecipe.Where(x => x.Recipe == r).ToList();

        r.SetSoftDeleted(Deps.DateTimeServiceLate.Moment, (await Deps.CurrentUserAccessor.GetUser()).Login);

        await context.SaveChangesAsync();

        Assert.Equal("Void", r.CreatedBy);
        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, r.ModifiedBy);

        Assert.Equal(Deps.DateTimeServiceEarly.Moment, r.CreatedOn);
        Assert.Equal(Deps.DateTimeServiceLate.Moment, r.ModifiedOn);

        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, r.DeletedBy);
        Assert.Equal(Deps.DateTimeServiceLate.Moment, r.DeletedOn);
        Assert.True(r.IsDeleted);
    }

    [Fact]
    public async Task Delete_sets_soft_delete_properties_offset()
    {
        using var context = Deps.FoodStuffsContextAuditable().Seed();

        var c = context.Category.First();

        c.SetSoftDeleted(Deps.DateTimeServiceLate.MomentWithOffset, (await Deps.CurrentUserAccessor.GetUser()).Login);

        await context.SaveChangesAsync();

        Assert.Equal("Void", c.CreatedBy);
        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, c.ModifiedBy);

        Assert.Equal(Deps.DateTimeServiceEarly.MomentWithOffset, c.CreatedOn);
        Assert.Equal(Deps.DateTimeServiceLate.MomentWithOffset, c.ModifiedOn);

        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, c.DeletedBy);
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
