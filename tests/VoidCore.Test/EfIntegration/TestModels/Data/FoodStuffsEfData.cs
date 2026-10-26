using VoidCore.EntityFramework;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Data;

public class FoodStuffsEfData : IFoodStuffsData
{
    public FoodStuffsEfData(FoodStuffsContext context, TimeProvider timeProvider, ICurrentUserAccessor currentUserAccessor)
    {
        Categories = new EfWritableRepository<Category>(context);
        CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
        Recipes = new EfWritableRepository<Recipe>(context).AddAuditability(timeProvider, currentUserAccessor);
    }

    public IWritableRepository<Category> Categories { get; }
    public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
    public IWritableRepository<Recipe> Recipes { get; }
}
