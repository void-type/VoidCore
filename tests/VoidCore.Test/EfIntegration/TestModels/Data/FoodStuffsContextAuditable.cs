using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
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
        /// Only needed during test seeding to prevent tampering with audit properties.
        /// </summary>
        public int SaveSeeding()
        {
            return base.SaveChanges(true);
        }

        // In practice, the following should be in a partial class to survive EF Database Scaffolding.
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetAuditableAndSoftDeleteProperties();
            SetAuditableAndSoftDeletePropertiesWithOffset();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetAuditableAndSoftDeleteProperties();
            SetAuditableAndSoftDeletePropertiesWithOffset();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditableAndSoftDeleteProperties()
        {
            var now = _dateTimeService.Moment;
            var user = _currentUserAccessor.User.Login;

            var auditableEntityEntries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable &&
                    (
                        e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted
                    ));

            foreach (var entry in auditableEntityEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((IAuditable)entry.Entity).SetAuditCreated(now, user);
                }

                if (entry.State == EntityState.Modified)
                {
                    ((IAuditable)entry.Entity).SetAuditModified(now, user);
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    ((ISoftDeletable)entry.Entity).SetSoftDeleted(now, user);
                }
            }
        }

        private void SetAuditableAndSoftDeletePropertiesWithOffset()
        {
            var now = _dateTimeService.MomentWithOffset;
            var user = _currentUserAccessor.User.Login;

            var auditableEntityEntries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditableWithOffset &&
                    (
                        e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted
                    ));

            foreach (var entry in auditableEntityEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((IAuditableWithOffset)entry.Entity).SetAuditCreated(now, user);
                }

                if (entry.State == EntityState.Modified)
                {
                    ((IAuditableWithOffset)entry.Entity).SetAuditModified(now, user);
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    ((ISoftDeletableWithOffset)entry.Entity).SetSoftDeleted(now, user);
                }
            }
        }
    }
}
