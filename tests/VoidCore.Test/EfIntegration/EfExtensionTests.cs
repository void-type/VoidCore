using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Responses.Collections;
using VoidCore.Test.EfIntegration.TestModels;
using Xunit;

namespace VoidCore.Test.EfIntegration;

public class EfExtensionTests
{
    [Fact]
    public async Task Get_page_of_results_using_ToItemSet_extension()
    {
        using var context = Deps.FoodStuffsContext().Seed();

        var paginationOptions = new PaginationOptions(2, 1);

        var page = await context.Recipe
            .OrderBy(x => x.Name)
            .ToItemSet(paginationOptions, CancellationToken.None);

        Assert.Equal(3, page.TotalCount);
        Assert.Equal("Recipe2", page.Items.Single().Name);
    }

    [Fact]
    public async Task ToItemSet_throws_when_paging_an_unordered_query()
    {
        using var context = Deps.FoodStuffsContext().Seed();

        await Assert.ThrowsAsync<InvalidOperationException>(() => context.Recipe
            .Where(x => x.Id > 0)
            .ToItemSet(new PaginationOptions(1, 2), CancellationToken.None));
    }

    [Fact]
    public async Task ToItemSet_allows_an_unordered_query_when_paging_is_disabled()
    {
        using var context = Deps.FoodStuffsContext().Seed();

        var page = await context.Recipe.ToItemSet(PaginationOptions.None, CancellationToken.None);

        Assert.Equal(3, page.Items.Count());
    }

    [Fact]
    public async Task ToItemSet_finds_the_ordering_through_filters_includes_tags_and_projections()
    {
        using var context = Deps.FoodStuffsContext().Seed();

        var page = await context.Recipe
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Where(x => x.Id > 0)
            .AsNoTracking()
            .TagWith("test")
            .Select(x => new { x.Id, x.Name })
            .ToItemSet(new PaginationOptions(1, 2), CancellationToken.None);

        Assert.Equal(2, page.Items.Count());
    }

    [Fact]
    public async Task ToItemSet_ignores_an_ordering_inside_a_subquery()
    {
        using var context = Deps.FoodStuffsContext().Seed();

        await Assert.ThrowsAsync<InvalidOperationException>(() => context.Recipe
            .Select(x => new { x.Id, FirstLink = x.CategoryRecipe.OrderBy(c => c.CategoryId).Select(c => c.CategoryId).FirstOrDefault() })
            .ToItemSet(new PaginationOptions(1, 2), CancellationToken.None));
    }

    [Fact]
    public async Task Get_page_of_all_results_using_ToItemSet_extension()
    {
        using var context = Deps.FoodStuffsContext().Seed();

        var paginationOptions = PaginationOptions.None;

        var page = await context.Recipe
            .OrderBy(x => x.Name)
            .ToItemSet(paginationOptions, CancellationToken.None);

        Assert.Equal(3, page.TotalCount);
        Assert.Equal(3, page.Items.Count());
    }
}
