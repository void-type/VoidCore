using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
using VoidCore.Model.Queries;
using VoidCore.Model.Responses.Collections;
using VoidCore.Test.AspNet.Data.TestModels.Data;
using VoidCore.Test.AspNet.Data.TestModels.Queries;

namespace VoidCore.Test.AspNet.Data.TestModels.Events
{
    public class ListRecipes
    {
        public class Handler : EventHandlerAbstract<Request, IItemSet<RecipeListItemDto>>
        {
            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<IItemSet<RecipeListItemDto>>> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var searchExpressions = new []
                {
                SearchCriteria.PropertiesContain<Recipe>(
                new SearchTerms(request.NameSearch),
                r => r.Name
                ),
                // TODO: Category search doesn't seem to work against SQL Server.
                SearchCriteria.PropertiesContain<Recipe>(
                new SearchTerms(request.CategorySearch),
                r => string.Join(" ", r.CategoryRecipe.Select(cr => cr.Category.Name))
                )
                };

                var allSearch = new RecipesSearchSpecification(searchExpressions, request.NameSort);

                var totalCount = await _data.Recipes.Count(allSearch);

                var pagedSearch = new RecipesSearchSpecification(searchExpressions, request.NameSort,
                    request.IsPagingEnabled, request.Page, request.Take);

                var recipes = await _data.Recipes.List(pagedSearch);

                return recipes
                    .Select(recipe => new RecipeListItemDto(
                        recipe.Id,
                        recipe.Name,
                        recipe.CategoryRecipe.Select(cr => cr.Category.Name)))
                    .ToItemSet(request.Page, request.Take, totalCount)
                    .Map(page => Result.Ok(page));
            }

            private readonly IFoodStuffsData _data;
        }

        public class Request
        {
            public Request(string nameSearch, string categorySearch, string nameSort, bool isPagingEnabled, int page, int take)
            {
                NameSearch = nameSearch;
                CategorySearch = categorySearch;
                NameSort = nameSort;
                IsPagingEnabled = isPagingEnabled;
                Page = page;
                Take = take;
            }

            public string NameSearch { get; }
            public string CategorySearch { get; }
            public string NameSort { get; }
            public bool IsPagingEnabled { get; }
            public int Page { get; }
            public int Take { get; }
        }

        public class RecipeListItemDto
        {
            public RecipeListItemDto(int id, string name, IEnumerable<string> categories)
            {
                Id = id;
                Name = name;
                Categories = categories;
            }

            public int Id { get; }
            public string Name { get; }
            public IEnumerable<string> Categories { get; }
        }

        public class Logger : ItemSetEventLogger<Request, RecipeListItemDto>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            protected override void OnBoth(Request request, IResult<IItemSet<RecipeListItemDto>> result)
            {
                Logger.Info($"CategorySearch: '{request.CategorySearch}'", $"NameSearch: '{request.NameSearch}'", $"NameSort: '{request.NameSort}'");
                base.OnBoth(request, result);
            }
        }
    }
}
