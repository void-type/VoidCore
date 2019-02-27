using System;
using System.Collections.Generic;
using VoidCore.Model.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Data
{
    public class Recipe : IAuditable
    {
        public List<CategoryRecipe> CategoryRecipe { get; set; } = new List<CategoryRecipe>();
        public int? CookTimeMinutes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Directions { get; set; }
        public int Id { get; set; }
        public string Ingredients { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Name { get; set; }
        public int? PrepTimeMinutes { get; set; }
    }
}
