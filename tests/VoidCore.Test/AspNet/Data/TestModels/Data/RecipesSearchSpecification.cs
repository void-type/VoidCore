using System;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Data
{
    public class RecipesSearchSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesSearchSpecification(Expression<Func<Recipe, bool>>[] criteria, string nameSort, bool pagingEnabled = false, int page = 1, int take = 1) : base(criteria)
        {
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");

            if (pagingEnabled)
            {
                ApplyPaging(page, take);
            }

            switch (nameSort?.ToLower())
            {
                case "ascending":
                    ApplyOrderBy(recipe => recipe.Name);
                    AddThenByDescending(recipe => recipe.CreatedOn);
                    break;

                case "descending":
                    ApplyOrderByDescending(recipe => recipe.Name);
                    AddThenBy(recipe => recipe.CreatedOn);
                    break;

                default:
                    ApplyOrderBy(recipe => recipe.Id);
                    break;
            }
        }
    }
}