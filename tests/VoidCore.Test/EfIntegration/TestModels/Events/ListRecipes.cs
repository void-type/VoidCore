using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Collections;
using VoidCore.Test.EfIntegration.TestModels.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Events
{
    public class ListRecipes
    {
        public class Handler : EventHandlerAbstract<Request, IItemSet<RecipeListItemDto>>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<IItemSet<RecipeListItemDto>>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var paginationOptions = new PaginationOptions(request.Page, request.Take, request.IsPagingEnabled);

                var searchCriteria = GetSearchCriteria(request);

                var allSearch = new RecipesSearchSpecification(searchCriteria);

                var totalCount = await _data.Recipes.Count(allSearch, cancellationToken);

                var pagedSearch = new RecipesSearchSpecification(
                    criteria: searchCriteria,
                    paginationOptions: paginationOptions,
                    sort: request.Sort);

                var recipes = await _data.Recipes.List(pagedSearch, cancellationToken);

                return recipes
                    .Select(recipe => new RecipeListItemDto(
                        id: recipe.Id,
                        name: recipe.Name,
                        categories: recipe.CategoryRecipe.Select(cr => cr.Category.Name)))
                    .ToItemSet(paginationOptions, totalCount)
                    .Map(Ok);
            }

            private Expression<Func<Recipe, bool>>[] GetSearchCriteria(Request request)
            {
                var searchCriteria = new List<Expression<Func<Recipe, bool>>>();

                if (!string.IsNullOrWhiteSpace(request.NameSearch))
                {
                    searchCriteria.Add(recipe => recipe.Name.ToLower().Contains(request.NameSearch.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(request.CategorySearch))
                {
                    searchCriteria.Add(recipe => recipe.CategoryRecipe.Any(cr => cr.Category.Name.ToLower().Contains(request.CategorySearch.ToLower())));
                }

                return searchCriteria.ToArray();
            }
        }

        public class Request
        {
            public Request(string nameSearch, string categorySearch, string sort, bool isPagingEnabled, int page, int take)
            {
                NameSearch = nameSearch;
                CategorySearch = categorySearch;
                Sort = sort;
                IsPagingEnabled = isPagingEnabled;
                Page = page;
                Take = take;
            }

            public string NameSearch { get; }
            public string CategorySearch { get; }
            public string Sort { get; }
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
                Logger.Info(
                    $"NameSearch: '{request.NameSearch}'",
                    $"CategorySearch: '{request.CategorySearch}'",
                    $"Sort: '{request.Sort}'",
                    $"IsPagingEnabled: '{request.IsPagingEnabled}'",
                    $"Page: '{request.Page}'",
                    $"Take: '{request.Take}'"
                );

                base.OnBoth(request, result);
            }
        }
    }
}
