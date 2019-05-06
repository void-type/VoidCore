using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Domain.RuleValidator;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;
using VoidCore.Test.AspNet.Data.TestModels.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Events
{
    public class SaveRecipe
    {
        public class Handler : EventHandlerAbstract<Request, UserMessageWithEntityId<int>>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<UserMessageWithEntityId<int>>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var byId = new RecipesByIdWithCategoriesSpecification(request.Id);

                var maybeRecipe = await _data.Recipes.Get(byId, cancellationToken);

                if (maybeRecipe.HasValue)
                {
                    return await maybeRecipe.Value
                        .Tee(r => TransferProperties(request, r))
                        .TeeAsync(r => _data.Recipes.Update(r, cancellationToken))
                        .TeeAsync(r => ManageCategories(request, r))
                        .MapAsync(r => Result.Ok(UserMessageWithEntityId.Create("Recipe updated.", r.Id)));
                }

                return await new Recipe()
                    .Tee(r => TransferProperties(request, r))
                    .TeeAsync(r => _data.Recipes.Add(r, cancellationToken))
                    .TeeAsync(r => ManageCategories(request, r))
                    .MapAsync(r => Result.Ok(UserMessageWithEntityId.Create("Recipe added.", r.Id)));
            }

            private void TransferProperties(Request request, Recipe recipe)
            {
                recipe.Name = request.Name;
                recipe.Ingredients = request.Ingredients;
                recipe.Directions = request.Directions;
                recipe.CookTimeMinutes = request.CookTimeMinutes;
                recipe.PrepTimeMinutes = request.PrepTimeMinutes;
            }

            private async Task ManageCategories(Request request, Recipe recipe)
            {
                var requested = request.Categories
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Select(n => n.ToLower().Trim())
                    .ToArray();

                var categoriesThatMatchRequestedSpec = new CategorySpecification(
                    c => requested.Contains(c.Name.ToLower().Trim()));

                var categoriesExist = (await _data.Categories.List(categoriesThatMatchRequestedSpec))
                    .Select(c => c.Name.ToLower().Trim());

                // Add categories that don't exist
                await requested
                    .Where(n => !categoriesExist.Contains(n))
                    .Select(n => new Category { Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(n) })
                    .TeeAsync(r => _data.Categories.AddRange(r));

                // Remove relations that are no longer needed
                await recipe.CategoryRecipe
                    .Where(r => !requested.Contains(r.Category.Name.ToLower().Trim()))
                    .TeeAsync(r => _data.CategoryRecipes.RemoveRange(r));

                // Add relations that don't exist
                await _data.Categories
                    .List(categoriesThatMatchRequestedSpec)
                    .MapAsync(categories => categories
                        .Where(c => !recipe.CategoryRecipe
                            .Select(r => r.Category.Name.ToLower().Trim())
                            .Contains(c.Name.ToLower().Trim()))
                        .Select(c => new CategoryRecipe
                        {
                            RecipeId = recipe.Id,
                            CategoryId = c.Id
                        }))
                    .TeeAsync(r => _data.CategoryRecipes.AddRange(r));
            }
        }

        public class Request
        {
            public Request(int id, string name, string ingredients, string directions, int? cookTimeMinutes, int? prepTimeMinutes, IEnumerable<string> categories)
            {
                Id = id;
                Name = name;
                Ingredients = ingredients;
                Directions = directions;
                CookTimeMinutes = cookTimeMinutes;
                PrepTimeMinutes = prepTimeMinutes;
                Categories = categories;
            }

            public int Id { get; }
            public string Name { get; }
            public string Ingredients { get; }
            public string Directions { get; }
            public int? CookTimeMinutes { get; }
            public int? PrepTimeMinutes { get; }
            public IEnumerable<string> Categories { get; }
        }

        public class RequestValidator : RuleValidatorAbstract<Request>
        {
            public RequestValidator()
            {
                CreateRule(new Failure("Please enter a name.", "name"))
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

                CreateRule(new Failure("Please enter ingredients.", "ingredients"))
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Ingredients));

                CreateRule(new Failure("Please enter directions.", "directions"))
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Directions));

                CreateRule(new Failure("Cook time must be positive.", "cookTimeMinutes"))
                    .InvalidWhen(entity => entity.CookTimeMinutes < 0);

                CreateRule(new Failure("Prep time must be positive.", "prepTimeMinutes"))
                    .InvalidWhen(entity => entity.PrepTimeMinutes < 0);
            }
        }

        public class Logger : UserMessageWithEntityIdEventLogger<Request, int>
        {
            public Logger(ILoggingService logger) : base(logger) { }
        }
    }
}
