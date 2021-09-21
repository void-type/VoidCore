using System;
using VoidCore.Model.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Data
{
    public class CategoryRecipe : IAuditableWithOffset, ISoftDeletableWithOffset
    {
        public int CategoryId { get; set; }
        public int RecipeId { get; set; }

        public Category Category { get; set; }
        public Recipe Recipe { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}
