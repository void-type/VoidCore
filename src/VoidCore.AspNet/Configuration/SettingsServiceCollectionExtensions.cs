using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.Domain.Events;
using VoidCore.Domain.Guards;
using VoidCore.Model.Configuration;

namespace VoidCore.AspNet.Configuration
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
        /// <typeparam name="TService">An interface or higher-level service to access the settings from</typeparam>
        /// <typeparam name="TSettings">The settings object type to pull from configuration</typeparam>
        /// <returns>The settings object to use during startup.</returns>
        public static TSettings AddSettingsSingleton<TService, TSettings>(this IServiceCollection services, IConfiguration configuration, bool root = false)
        where TSettings : class, TService, new()
        where TService : class
        {
            var settings = GetSettings<TSettings>(configuration, root);
            services.AddSingleton<TService>(x => settings);
            return settings;
        }

        /// <summary>
        /// Search assemblies for domain pipeline components and registers them with the DI container by concrete name.
        /// This is a convenience method that has a cost of performance. Components are registered as transient which increases garbage collector pressure during heavy loads.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="assembliesToSearch">An array of assemblies to search for domain components in</param>
        public static void FindAndRegisterDomainEvents(this IServiceCollection services, params Assembly[] assembliesToSearch)
        {
            assembliesToSearch.EnsureNotNullOrEmpty(nameof(assembliesToSearch));

            var domainEventInterfaces = new[] {
                typeof (IEventHandler<,>),
                typeof (IRequestValidator<>),
                typeof (IPostProcessor<,>)
            };

            foreach (var @interface in domainEventInterfaces)
            {
                var matchingConcretes = assembliesToSearch
                    .Distinct()
                    .SelectMany(assembly => assembly.DefinedTypes)
                    .Where(type => type.IsConcrete())
                    .Where(type => type.ImplementsGenericInterface(@interface))
                    .ToList();

                foreach (var type in matchingConcretes)
                {
                    services.AddTransient(type);
                }
            }
        }

        private static TSettings GetSettings<TSettings>(IConfiguration configuration, bool root)
        {
            if (!root)
            {
                var sectionName = typeof(TSettings).GetTypeNameWithoutEnding("settings");
                configuration = configuration.GetSection(sectionName);
            }

            return configuration.Get<TSettings>(options => options.BindNonPublicProperties = true);
        }

        private static bool IsConcrete(this TypeInfo type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        private static bool ImplementsGenericInterface(this TypeInfo type, Type @interface)
        {
            return type.GetTypeInfo()
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .Select(i => i.GetGenericTypeDefinition())
                .Contains(@interface);
        }
    }
}
