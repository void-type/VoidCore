using VoidCore.AspNet.Data;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using VoidCore.Model.Time;

namespace VoidCore.Test.AspNet.Data.TestModels.Data
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public FoodStuffsEfData(FoodStuffsContext context, IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        {
            Categories = new EfWritableRepository<Category>(context);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
            Recipes = new AuditableRepositoryDecorator<Recipe>(new EfWritableRepository<Recipe>(context), now, currentUserAccessor);
            Users = new EfQueryRepository<User>(context);
        }

        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Recipe> Recipes { get; }
        public IReadOnlyRepository<User> Users { get; }
    }
}
