using VoidCore.Model.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Data;

public class RecipesByIdWithCategoriesSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesByIdWithCategoriesSpecification(int id) : base()
    {
        AddCriteria(r => r.Id == id);

        AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");
    }
}
