using VoidCore.Model.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Data;

public class Category : IAuditableWithOffset, ISoftDeletableWithOffset
{
    public List<CategoryRecipe> CategoryRecipe { get; set; } = [];
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTimeOffset ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
    public string DeletedBy { get; set; } = string.Empty;
    public DateTimeOffset? DeletedOn { get; set; }
}
