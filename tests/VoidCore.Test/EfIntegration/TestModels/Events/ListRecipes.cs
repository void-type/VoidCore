using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Text;
using VoidCore.Test.EfIntegration.TestModels.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Events;

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
            var pagedSearch = new RecipesSearchSpecification(
                criteria: GetSearchCriteria(request),
                paginationOptions: request.GetPaginationOptions(),
                sort: request.Sort,
                sortDesc: request.SortDesc);

            return await _data.Recipes
                 .ListPage(pagedSearch, cancellationToken)
                 .SelectAsync(recipe => new RecipeListItemDto(
                     id: recipe.Id,
                     name: recipe.Name,
                     categories: recipe.CategoryRecipe.Select(cr => cr.Category.Name)))
                 .MapAsync(Ok);
        }

        private static Expression<Func<Recipe, bool>>[] GetSearchCriteria(Request request)
        {
            var searchCriteria = new List<Expression<Func<Recipe, bool>>>();

            if (!request.NameSearch.IsNullOrWhiteSpace())
            {
                searchCriteria.Add(recipe => recipe.Name.Contains(request.NameSearch, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!request.CategorySearch.IsNullOrWhiteSpace())
            {
                searchCriteria.Add(recipe => recipe.CategoryRecipe.Any(cr => cr.Category.Name.Contains(request.CategorySearch, StringComparison.CurrentCultureIgnoreCase)));
            }

            return searchCriteria.ToArray();
        }
    }

    public class Request : IPaginatedRequest
    {
        public Request(string nameSearch, string categorySearch, string sort, bool sortDesc, bool isPagingEnabled, int page, int take)
        {
            NameSearch = nameSearch;
            CategorySearch = categorySearch;
            Sort = sort;
            SortDesc = sortDesc;
            IsPagingEnabled = isPagingEnabled;
            Page = page;
            Take = take;
        }

        public string NameSearch { get; }
        public string CategorySearch { get; }
        public string Sort { get; }
        public bool SortDesc { get; }
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

    public class RequestLogger : RequestLoggerAbstract<Request>
    {
        public RequestLogger(ILogger<RequestLogger> logger) : base(logger) { }

        public override void Log(Request request)
        {
            Logger.LogInformation("Requested. NameSearch: '{NameSearch}' CategorySearch: '{CategorySearch}' Sort: '{Sort}' IsPagingEnabled: '{IsPagingEnabled}' Page: '{Page}' Take: '{Take}'",
                request.NameSearch,
                request.CategorySearch,
                request.Sort,
                request.IsPagingEnabled,
                request.Page,
                request.Take
            );
        }
    }

    public class ResponseLogger : ItemSetEventLogger<Request, RecipeListItemDto>
    {
        public ResponseLogger(ILogger<ResponseLogger> logger) : base(logger) { }
    }
}
