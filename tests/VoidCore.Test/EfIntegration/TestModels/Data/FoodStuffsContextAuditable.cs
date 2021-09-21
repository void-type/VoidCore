using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.EntityFramework;
using VoidCore.Model.Auth;
using VoidCore.Model.Time;

namespace VoidCore.Test.EfIntegration.TestModels.Data
{
    public class FoodStuffsContextAuditable : DbContext
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public FoodStuffsContextAuditable(DbContextOptions<FoodStuffsContextAuditable> options, IDateTimeService dateTimeService, ICurrentUserAccessor currentUserAccessor) : base(options)
        {
            _dateTimeService = dateTimeService;
            _currentUserAccessor = currentUserAccessor;
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

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Directions).IsRequired();

                entity.Property(e => e.Ingredients).IsRequired();

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();
            });
        }

        /// <summary>
        /// Only needed during unit test seeding to prevent tampering with audit properties.
        /// </summary>
        public int SaveSeeding()
        {
            return base.SaveChanges(true);
        }

        // In practice, the following should be in a partial class to survive EF Database Scaffolding.
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.Entries().SetAllAuditableProperties(_dateTimeService, _currentUserAccessor.User.Login);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ChangeTracker.Entries().SetAllAuditableProperties(_dateTimeService, _currentUserAccessor.User.Login);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
