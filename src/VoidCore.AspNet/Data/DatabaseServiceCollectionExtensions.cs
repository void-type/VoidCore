using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VoidCore.AspNet.Data
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
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if null is passed for connectionString.</exception>
        public static void AddSqlServerDbContext<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Application is not properly configured. Connection string is either empty or not found.");
            }

            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
