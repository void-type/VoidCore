using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Test.EfIntegration.TestModels;
using VoidCore.Test.EfIntegration.TestModels.Data;
using Xunit;

namespace VoidCore.Test.EfIntegration;

public class EfAuditableDbContextTests
{
    [Fact]
    public async Task Add_sets_creation_properties()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var r = new Recipe() { Name = "new recipe", Directions = "", Ingredients = "" };
        context.Recipe.Add(r);
        await context.SaveChangesAsync();

        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, r.CreatedBy);
        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, r.ModifiedBy);

        Assert.Equal(Deps.Late, r.CreatedOn);
        Assert.Equal(Deps.Late, r.ModifiedOn);
        Assert.Equal(TimeSpan.Zero, r.CreatedOn.Offset);
        Assert.Equal(TimeSpan.Zero, r.ModifiedOn.Offset);
    }

    [Fact]
    public async Task Add_sets_creation_properties_for_all_auditable_entities()
    {
        using var context = Deps.FoodStuffsContextAuditable();

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

        Assert.Equal(Deps.Late, c.CreatedOn);
        Assert.Equal(Deps.Late, c.ModifiedOn);
    }

    [Fact]
    public async Task Modify_sets_modification_properties()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var r = context.Recipe.First();

        r.Name = "new name";
        await context.SaveChangesAsync();

        Assert.Equal("Void", r.CreatedBy);
        Assert.Equal((await Deps.CurrentUserAccessor.GetUser()).Login, r.ModifiedBy);

        Assert.Equal(Deps.Early, r.CreatedOn);
        Assert.Equal(Deps.Late, r.ModifiedOn);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Setting_IsDeleted_stamps_soft_delete_properties(bool softDeletableFirst)
    {
        using var context = Deps.FoodStuffsContextAuditable(softDeletableFirst: softDeletableFirst);

        var r = context.Recipe.First();
        var id = r.Id;

        r.IsDeleted = true;

        await context.SaveChangesAsync();

        Assert.Equal(EntityState.Unchanged, context.Entry(r).State);

        // The row still exists in the database.
        var stored = await context.Recipe.AsNoTracking().IncludeSoftDeleted().SingleAsync(x => x.Id == id);

        Assert.True(stored.IsDeleted);
        Assert.Equal("SingleUser", stored.DeletedBy);
        Assert.Equal(Deps.Late, stored.DeletedOn);
        Assert.Equal(TimeSpan.Zero, stored.DeletedOn!.Value.Offset);

        // Soft-deleting is a modification, so audit properties are set regardless of interceptor order.
        Assert.Equal("Void", stored.CreatedBy);
        Assert.Equal(Deps.Early, stored.CreatedOn);
        Assert.Equal("SingleUser", stored.ModifiedBy);
        Assert.Equal(Deps.Late, stored.ModifiedOn);
    }

    [Fact]
    public void Synchronous_save_stamps_soft_delete_properties()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var r = context.Recipe.First();
        var id = r.Id;

        r.IsDeleted = true;
        context.SaveChanges();

        var stored = context.Recipe.AsNoTracking().IncludeSoftDeleted().Single(x => x.Id == id);

        Assert.True(stored.IsDeleted);
        Assert.Equal("SingleUser", stored.DeletedBy);
        Assert.Equal(Deps.Late, stored.DeletedOn);
    }

    [Fact]
    public async Task Soft_deleting_a_new_entity_stamps_soft_delete_properties()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var r = new Recipe() { Name = "born deleted", Directions = "", Ingredients = "", IsDeleted = true };
        context.Recipe.Add(r);
        await context.SaveChangesAsync();

        var stored = await context.Recipe.AsNoTracking().OnlySoftDeleted().SingleAsync(x => x.Name == "born deleted");
        Assert.Equal("SingleUser", stored.DeletedBy);
        Assert.Equal(Deps.Late, stored.DeletedOn);
    }

    [Fact]
    public async Task Existing_soft_delete_stamps_are_preserved()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var r = context.Recipe.First();
        var deletedOn = new DateTimeOffset(2010, 5, 5, 5, 5, 5, TimeSpan.Zero);

        r.IsDeleted = true;
        r.DeletedOn = deletedOn;
        r.DeletedBy = "importer";
        await context.SaveChangesAsync();

        r.Name = "changed after deletion";
        await context.SaveChangesAsync();

        var stored = await context.Recipe.AsNoTracking().IncludeSoftDeleted().SingleAsync(x => x.Id == r.Id);
        Assert.Equal(deletedOn, stored.DeletedOn);
        Assert.Equal("importer", stored.DeletedBy);
    }

    [Fact]
    public async Task Soft_deleted_entities_are_hidden_unless_included()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var total = await context.Recipe.CountAsync();
        var r = await context.Recipe.FirstAsync();

        r.IsDeleted = true;
        await context.SaveChangesAsync();

        Assert.Equal(total - 1, await context.Recipe.CountAsync());
        Assert.DoesNotContain(await context.Recipe.ToListAsync(), x => x.Id == r.Id);

        Assert.Equal(total, await context.Recipe.IncludeSoftDeleted().CountAsync());

        var recycleBin = await context.Recipe.OnlySoftDeleted().ToListAsync();
        Assert.Equal(r.Id, Assert.Single(recycleBin).Id);
    }

    [Fact]
    public async Task Soft_deleted_entities_can_be_restored()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var total = await context.Recipe.CountAsync();
        var r = await context.Recipe.FirstAsync();

        r.IsDeleted = true;
        await context.SaveChangesAsync();

        var deleted = await context.Recipe.OnlySoftDeleted().SingleAsync();
        deleted.IsDeleted = false;
        await context.SaveChangesAsync();

        Assert.Equal(total, await context.Recipe.CountAsync());
        Assert.Empty(await context.Recipe.OnlySoftDeleted().ToListAsync());

        var restored = await context.Recipe.AsNoTracking().SingleAsync(x => x.Id == r.Id);
        Assert.False(restored.IsDeleted);
        Assert.Null(restored.DeletedOn);
        Assert.Equal(string.Empty, restored.DeletedBy);
    }

    [Fact]
    public async Task Remove_permanently_deletes_by_default()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var total = await context.Recipe.CountAsync();
        var r = await context.Recipe.FirstAsync();

        context.Recipe.Remove(r);
        await context.SaveChangesAsync();

        Assert.Equal(total - 1, await context.Recipe.IncludeSoftDeleted().CountAsync());
    }

    [Fact]
    public async Task Remove_permanently_deletes_a_soft_deleted_entity_when_the_recycle_bin_is_required()
    {
        using var context = Deps.FoodStuffsContextAuditable(requireRecycleBin: true);

        var total = await context.Recipe.CountAsync();
        var r = await context.Recipe.FirstAsync();

        r.IsDeleted = true;
        await context.SaveChangesAsync();

        var deleted = await context.Recipe.OnlySoftDeleted().SingleAsync();
        context.Recipe.Remove(deleted);
        await context.SaveChangesAsync();

        Assert.Empty(await context.Recipe.OnlySoftDeleted().ToListAsync());
        Assert.Equal(total - 1, await context.Recipe.IncludeSoftDeleted().CountAsync());
    }

    [Fact]
    public async Task Remove_throws_for_entities_that_havent_been_soft_deleted_when_the_recycle_bin_is_required()
    {
        using var context = Deps.FoodStuffsContextAuditable(requireRecycleBin: true);

        var total = await context.Recipe.CountAsync();
        var r = await context.Recipe.FirstAsync();

        context.Recipe.Remove(r);

        await Assert.ThrowsAsync<InvalidOperationException>(() => context.SaveChangesAsync());
        Assert.Throws<InvalidOperationException>(() => context.SaveChanges());

        context.ChangeTracker.Clear();
        Assert.Equal(total, await context.Recipe.IncludeSoftDeleted().CountAsync());
    }

    [Fact]
    public async Task Soft_deleting_and_removing_in_the_same_save_throws_when_the_recycle_bin_is_required()
    {
        using var context = Deps.FoodStuffsContextAuditable(requireRecycleBin: true);

        var r = await context.Recipe.FirstAsync();

        r.IsDeleted = true;
        context.Recipe.Remove(r);

        await Assert.ThrowsAsync<InvalidOperationException>(() => context.SaveChangesAsync());
    }

    [Fact]
    public async Task Required_recycle_bin_doesnt_affect_entities_that_arent_soft_deletable()
    {
        using var context = Deps.FoodStuffsContextAuditable(requireRecycleBin: true);

        var cr = await context.CategoryRecipe.FirstAsync();
        var count = await context.CategoryRecipe.CountAsync();

        context.CategoryRecipe.Remove(cr);
        await context.SaveChangesAsync();

        Assert.Equal(count - 1, await context.CategoryRecipe.CountAsync());
    }

    [Fact]
    public async Task Soft_deleting_many_entities_stamps_all_of_them()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var recipes = await context.Recipe.ToListAsync();
        var categories = await context.Category.ToListAsync();

        recipes.ForEach(x => x.IsDeleted = true);
        categories.ForEach(x => x.IsDeleted = true);
        await context.SaveChangesAsync();

        Assert.Empty(await context.Recipe.ToListAsync());
        Assert.Empty(await context.Category.ToListAsync());
        Assert.Equal(recipes.Count, await context.Recipe.OnlySoftDeleted().CountAsync());
        Assert.Equal(categories.Count, await context.Category.OnlySoftDeleted().CountAsync());
        Assert.All(recipes, x => Assert.Equal(Deps.Late, x.DeletedOn));
    }

    [Fact]
    public async Task Remove_still_deletes_entities_that_arent_soft_deletable()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var cr = context.CategoryRecipe.First();
        var count = context.CategoryRecipe.Count();

        context.CategoryRecipe.Remove(cr);
        await context.SaveChangesAsync();

        Assert.Equal(count - 1, context.CategoryRecipe.Count());
    }

    [Fact]
    public async Task Saving_doesnt_throw_exception_on_ignored_entities()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var cr = context.CategoryRecipe.First();

        cr.Name = "new name";
        await context.SaveChangesAsync();
    }

    [Fact]
    public void Synchronous_save_sets_audit_properties()
    {
        using var context = Deps.FoodStuffsContextAuditable();

        var added = new Recipe() { Name = "new recipe", Directions = "", Ingredients = "" };
        context.Recipe.Add(added);

        var modified = context.Recipe.First(x => x.Name == "Recipe1");
        modified.Name = "new name";

        context.SaveChanges();

        Assert.Equal("SingleUser", added.CreatedBy);
        Assert.Equal(Deps.Late, added.CreatedOn);
        Assert.Equal("SingleUser", modified.ModifiedBy);
        Assert.Equal(Deps.Late, modified.ModifiedOn);
        Assert.Equal("Void", modified.CreatedBy);
        Assert.Equal(Deps.Early, modified.CreatedOn);
    }
}
