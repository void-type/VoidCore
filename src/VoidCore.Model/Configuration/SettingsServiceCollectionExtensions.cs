using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.Model.Text;

namespace VoidCore.Model.Configuration
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
            var settings = GetSettings<TSettings>(configuration, root);
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
        /// <typeparam name="TSettingsInterface">An interface or higher-level service to access the settings from</typeparam>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.</returns>
        public static TSettings AddSettingsSingleton<TSettingsInterface, TSettings>(this IServiceCollection services, IConfiguration configuration, bool root = false)
        where TSettingsInterface : class
        where TSettings : class, TSettingsInterface, new()
        {
            var settings = GetSettings<TSettings>(configuration, root);
            services.AddSingleton<TSettingsInterface>(_ => settings);
            return settings;
        }

        private static TSettings GetSettings<TSettings>(IConfiguration configuration, bool root)
        {
            return GetSettingsConfiguration<TSettings>(configuration, root)
                .Get<TSettings>(options => options.BindNonPublicProperties = true);
        }

        private static IConfiguration GetSettingsConfiguration<TSettings>(IConfiguration configuration, bool root)
        {
            if (root)
            {
                return configuration;
            }

            var sectionName = typeof(TSettings).GetTypeNameWithoutEnding("settings");
            return configuration.GetSection(sectionName);
        }
    }
}
