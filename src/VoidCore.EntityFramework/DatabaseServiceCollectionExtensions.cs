using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.Domain.Guards;

namespace VoidCore.EntityFramework
{
    /// <summary>
    /// Configuration for databases.
    /// </summary>
    public static class DatabaseServiceCollectionExtensions
    {
        /// <summary>
        /// Add a DbContext to the DI container using SQL server settings.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="connectionString">The connection string to send to the DbContext</param>
        /// <typeparam name="TDbContext">The concrete type of DbContext to add to the DI container</typeparam>
        public static void AddSqlServerDbContext<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            connectionString.EnsureNotNullOrEmpty(nameof(connectionString), "Connection string not found in application configuration.");

            services.AddDbContextPool<TDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
