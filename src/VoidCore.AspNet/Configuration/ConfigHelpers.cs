using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VoidCore.AspNet.Configuration
{
    /// <summary>
    /// Helpers to assist in configuring the application.
    /// </summary>
    public static class ConfigHelpers
    {
        /// <summary>
        /// Strips an ending from a class type name. This is useful for convention-based naming to replace hardcoded strings.
        /// Ex: AuthorizationSettings, "settings" => "Authorization"
        /// Ex: Authorization, "settings" => "Authorization"
        /// Ex: AuthorizationSettings, null => "AuthorizationSettings"
        /// </summary>
        /// <param name="type">The type to get the name from</param>
        /// <param name="ending">The ending to remove from the class type name</param>
        /// <returns>Type name with the ending removed</returns>
        public static string StripEndingFromType(Type type, string ending)
        {
            var rawName = type.Name;
            var nameEnd = rawName.Length;

            if (ending == null)
            {
                return rawName.Substring(0, nameEnd);
            }

            var lastIndexOfEnding = rawName.ToLower().LastIndexOf(ending, StringComparison.Ordinal);

            if (lastIndexOfEnding > -1)
            {
                nameEnd = lastIndexOfEnding;
            }

            return rawName.Substring(0, nameEnd);
        }

        /// <summary>
        /// Pulls a settings object from configuration and adds it as a singleton to the DI container.
        /// Uses naming conventions of the Settings class to find the config section.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <param name="root">When true, the naming conventions are ignored and settings are assumed to be at the root of the config</param>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.</returns>
        public static TSettings AddSettingsSingleton<TSettings>(this IServiceCollection services, IConfiguration configuration, bool root = false)
        where TSettings : class, new()
        {
            if (!root)
            {
                var sectionName = ConfigHelpers.StripEndingFromType(typeof(TSettings), "settings");
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
        /// <param name="root">When true, the naming conventions are ignored and settings are assumed to be at the root of the config</param>
        /// <typeparam name="TService">An interface or higher-level service to access the settings from</typeparam>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.</returns>
        public static TSettings AddSettingsSingleton<TService, TSettings>(this IServiceCollection services, IConfiguration configuration, bool root = false)
        where TSettings : class, TService, new()
        where TService : class
        {
            if (!root)
            {
                var sectionName = ConfigHelpers.StripEndingFromType(typeof(TSettings), "settings");
                configuration = configuration.GetSection(sectionName);
            }

            var settings = configuration.Get<TSettings>(options => options.BindNonPublicProperties = true);
            services.AddSingleton<TService>(x => settings);
            return settings;
        }
    }
}
