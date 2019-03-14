using VoidCore.Model.Queries;
using VoidCore.Test.AspNet.Data.TestModels.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Queries
{
    public class RecipesByIdWithCategoriesSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesByIdWithCategoriesSpecification(int id) : base(r => r.Id == id)
        {
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");
        }
    }
}
