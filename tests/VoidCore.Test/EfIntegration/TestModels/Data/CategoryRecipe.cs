namespace VoidCore.Test.EfIntegration.TestModels.Data;

public class CategoryRecipe
{
    public int CategoryId { get; set; }
    public int RecipeId { get; set; }

    public Category Category { get; set; } = null!;
    public Recipe Recipe { get; set; } = null!;

    public string? Name { get; set; }
}
