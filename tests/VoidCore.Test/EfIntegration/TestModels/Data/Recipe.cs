using VoidCore.Model.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Data;

public class Recipe : IAuditable, ISoftDeletable
{
    public List<CategoryRecipe> CategoryRecipe { get; set; } = [];
    public int? CookTimeMinutes { get; set; }
    public string Directions { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Ingredients { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? PrepTimeMinutes { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
    public string DeletedBy { get; set; } = string.Empty;
    public DateTime? DeletedOn { get; set; }
}
