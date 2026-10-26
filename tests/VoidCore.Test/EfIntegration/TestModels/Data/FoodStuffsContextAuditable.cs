using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;

namespace VoidCore.Test.EfIntegration.TestModels.Data;

public class FoodStuffsContextAuditable : DbContext
{
    public FoodStuffsContextAuditable(DbContextOptions<FoodStuffsContextAuditable> options) : base(options)
    {
    }

    public virtual DbSet<Category> Category { get; set; }
    public virtual DbSet<CategoryRecipe> CategoryRecipe { get; set; }
    public virtual DbSet<Recipe> Recipe { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category", "dbo");

            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<CategoryRecipe>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.CategoryId });

            entity.ToTable("CategoryRecipe", "dbo");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.CategoryRecipe)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryRecipe_Category");

            entity.HasOne(d => d.Recipe)
                .WithMany(p => p.CategoryRecipe)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryRecipe_Recipe");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable("Recipe", "dbo");

            entity.Property(e => e.Directions).IsRequired();

            entity.Property(e => e.Ingredients).IsRequired();

            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
