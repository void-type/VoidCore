using VoidCore.Model.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Data
{
    public class RecipesByIdWithCategoriesSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesByIdWithCategoriesSpecification(int id) : base(r => r.Id == id)
        {
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");
        }
    }
}
