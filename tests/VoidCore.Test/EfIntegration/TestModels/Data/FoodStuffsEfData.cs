﻿using VoidCore.EntityFramework;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;

namespace VoidCore.Test.EfIntegration.TestModels.Data
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public FoodStuffsEfData(FoodStuffsContext context, ILoggingStrategy loggingStrategy, IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        {
            Categories = new EfWritableRepository<Category>(context, loggingStrategy);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context, loggingStrategy);
            Recipes = new EfWritableRepository<Recipe>(context, loggingStrategy).AddAuditability(now, currentUserAccessor);
        }

        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Recipe> Recipes { get; }
    }
}
