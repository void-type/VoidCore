using VoidCore.Model.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Data;

public interface IFoodStuffsData
{
    IWritableRepository<Category> Categories { get; }
    IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
    IWritableRepository<Recipe> Recipes { get; }
}
