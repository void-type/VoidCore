using System;
using System.Linq.Expressions;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;

namespace VoidCore.Test.EfIntegration.TestModels.Data;

public class RecipesSearchSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria, string sort = null) : this(criteria, PaginationOptions.None, sort) { }

    public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria, PaginationOptions paginationOptions, string sort = null, bool sortDesc = false) : base(criteria)
    {
        AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");

        ApplyPaging(paginationOptions);

        switch (sort)
        {
            case "name":
                AddOrderBy(recipe => recipe.Name, sortDesc);
                AddOrderBy(recipe => recipe.CreatedOn);
                break;

            default:
                AddOrderBy(recipe => recipe.Id);
                break;
        }
    }
}
