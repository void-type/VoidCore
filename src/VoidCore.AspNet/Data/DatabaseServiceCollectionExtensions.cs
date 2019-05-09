using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using VoidCore.Domain.Guards;

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
        /// <param name="environment">The hosting environment</param>
        /// <param name="connectionString">The connection string to send to the DbContext</param>
        /// <typeparam name="TDbContext">The concrete type of DbContext to add to the DI container</typeparam>
        public static void AddSqlServerDbContext<TDbContext>(this IServiceCollection services, IHostingEnvironment environment, string connectionString) where TDbContext : DbContext
        {
            connectionString.EnsureNotNullOrEmpty(nameof(connectionString), "Connection string not found in application configuration.");

            services.AddDbContextPool<TDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

                if (environment.IsDevelopment())
                {
                    // TODO: .Net Core 3.0 will have a new way of doing this. Can ignore CS0618 warning for now.
                    var consoleLoggerFactory = new LoggerFactory(new[]
                    {
                        new ConsoleLoggerProvider(
                            (category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information,
                            false)
                    });

                    options.UseLoggerFactory(consoleLoggerFactory);
                }
            });
        }
    }
}
