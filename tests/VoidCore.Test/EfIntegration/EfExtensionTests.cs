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
