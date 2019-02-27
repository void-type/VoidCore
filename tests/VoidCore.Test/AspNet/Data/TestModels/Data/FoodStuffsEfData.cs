using VoidCore.AspNet.Data;
using VoidCore.Model.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Data
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Recipe> Recipes { get; }

        public FoodStuffsEfData(FoodStuffsContext context)
        {
            Categories = new EfWritableRepository<Category>(context);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
            Recipes = new EfWritableRepository<Recipe>(context);
        }
    }
}
