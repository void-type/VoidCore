using VoidCore.EntityFramework;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using VoidCore.Model.Time;

namespace VoidCore.Test.EfIntegration.TestModels.Data;

public class FoodStuffsEfData : IFoodStuffsData
{
    public FoodStuffsEfData(FoodStuffsContext context, IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
    {
        Categories = new EfWritableRepository<Category>(context);
        CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
        Recipes = new EfWritableRepository<Recipe>(context).AddAuditability(now, currentUserAccessor);
    }

    public IWritableRepository<Category> Categories { get; }
    public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
    public IWritableRepository<Recipe> Recipes { get; }
}
