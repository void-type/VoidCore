using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VoidCore.AspNet.Settings
{
    /// <summary>
    /// Settings configuration extension methods for service collections.
    /// </summary>
    public static class SettingsServiceCollectionExtensions
    {
        /// <summary>
        /// Pulls a settings object from configuration and adds it as a singleton to the DI container. Uses naming
        /// conventions of the Settings class to find the config section.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <param name="root">
        /// When true, the naming conventions are ignored and settings are assumed to be at the root of the config
        /// </param>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.</returns>
        public static TSettings AddSettingsSingleton<TSettings>(this IServiceCollection services, IConfiguration configuration, bool root = false)
        where TSettings : class, new()
        {
            if (!root)
            {
                var sectionName = ConventionHelpers.StripEndingFromType(typeof(TSettings), "settings");
                configuration = configuration.GetSection(sectionName);
            }

            var settings = configuration.Get<TSettings>(options => options.BindNonPublicProperties = true);
            services.AddSingleton(settings);
            return settings;
        }

        /// <summary>
        /// Pulls a settings object from configuration and adds it as a singleton to the DI container via an interface.
        /// Uses naming conventions of the Settings class to find the config section.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <param name="root">
        /// When true, the naming conventions are ignored and settings are assumed to be at the root of the config
        /// </param>
        /// <typeparam name="TService">An interface or higher-level service to access the settings from</typeparam>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.</returns>
        public static TSettings AddSettingsSingleton<TService, TSettings>(this IServiceCollection services, IConfiguration configuration, bool root = false)
        where TSettings : class, TService, new()
        where TService : class
        {
            if (!root)
            {
                var sectionName = ConventionHelpers.StripEndingFromType(typeof(TSettings), "settings");
                configuration = configuration.GetSection(sectionName);
            }

            var settings = configuration.Get<TSettings>(options => options.BindNonPublicProperties = true);
            services.AddSingleton<TService>(x => settings);
            return settings;
        }
    }
}
