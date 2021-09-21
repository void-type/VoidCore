using System;
using System.Collections.Generic;
using VoidCore.Model.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Data
{
    public class Category : IAuditableWithOffset, ISoftDeletableWithOffset
    {
        public List<CategoryRecipe> CategoryRecipe { get; set; } = new List<CategoryRecipe>();
        public int Id { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}
