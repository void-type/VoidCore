using Microsoft.Extensions.Configuration;
using VoidCore.Model.Guards;
using VoidCore.Model.Text;

namespace VoidCore.Model.Configuration;

/// <summary>
/// Extensions for IConfiguration.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets a connection string from configuration using the type name of the DbContext and throws an exception when string isn't found.
    /// The type name follows the EF scaffolding convention. For example, FoodStuffsContext will look for FoodStuffs.
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <typeparam name="T">The type of DbContext</typeparam>
    /// <returns></returns>
    public static string GetRequiredConnectionString<T>(this IConfiguration configuration)
    {
        var name = typeof(T).GetTypeNameWithoutEnding("context");

        return configuration.GetRequiredConnectionString(name);
    }

    /// <summary>
    /// Gets a connection string from configuration by name and throws an exception when string isn't found.
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <param name="connectionStringName">The name of the connection string</param>
    /// <returns></returns>
    public static string GetRequiredConnectionString(this IConfiguration configuration, string connectionStringName)
    {
        const string message = "Connection string not found in application configuration.";

        return configuration
            .GetConnectionString(connectionStringName)
            .EnsureNotNullOrEmpty(message);
    }
}
