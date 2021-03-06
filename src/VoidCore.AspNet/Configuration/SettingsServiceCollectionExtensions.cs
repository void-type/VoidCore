﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using VoidCore.Domain.Events;
using VoidCore.Domain.Guards;
using VoidCore.Model.Text;

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

        /// <summary>
        /// Search assemblies for domain pipeline components and registers them with the DI container by concrete name.
        /// This is a convenience method that has a cost of uniformity. All components are registered with the specified lifetime.
        /// You can override how a service gets registered by re-registering it under a different lifetime manually later in Startup.cs.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="lifetime">The ServiceLifetime to register the services for.</param>
        /// <param name="assembliesToSearch">An array of assemblies to search for domain components in</param>
        public static void FindAndRegisterDomainEvents(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assembliesToSearch)
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
                    .Where(type => type.IsConcrete() && type.ImplementsGenericInterface(@interface))
                    .ToList();

                foreach (var type in matchingConcretes)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(type);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(type);
                            break;
                        default:
                            services.AddTransient(type);
                            break;
                    }
                }
            }
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
