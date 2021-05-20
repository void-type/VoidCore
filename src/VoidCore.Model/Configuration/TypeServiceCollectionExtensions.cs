using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Configuration
{
    /// <summary>
    /// Settings configuration extension methods for service collections.
    /// </summary>
    public static class TypeServiceCollectionExtensions
    {
        /// <summary>
        /// Search assemblies for domain event components and registers them with the DI container by concrete name.
        /// This is a convenience method that has a cost of uniformity. All components are registered with the specified lifetime.
        /// You can override how a service gets registered by re-registering it under a different lifetime manually later in Startup.cs.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="lifetime">The ServiceLifetime to register the services for.</param>
        /// <param name="assembliesToSearch">An array of assemblies to search for domain components in</param>
        public static void AddDomainEvents(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assembliesToSearch)
        {
            var domainEventTypes = new[] {
                typeof (IEventHandler<,>),
                typeof (IRequestLogger<>),
                typeof (IRequestValidator<>),
                typeof (IPostProcessor<,>)};

            AddImplementationsOfBaseType(services, lifetime, domainEventTypes, assembliesToSearch);
        }

        /// <summary>
        /// Search assemblies for implementations of classes and interfaces and registers them with the DI container by concrete name.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="lifetime">The ServiceLifetime to register the services for.</param>
        /// <param name="baseTypes">An array of base types or interfaces to register concrete implementations of</param>
        /// <param name="assembliesToSearch">An array of assemblies to search for domain components in</param>
        public static void AddImplementationsOfBaseType(this IServiceCollection services, ServiceLifetime lifetime, Type[] baseTypes, params Assembly[] assembliesToSearch)
        {
            assembliesToSearch.EnsureNotNullOrEmpty(nameof(assembliesToSearch));

            foreach (var baseType in baseTypes)
            {
                // DI can register concrete types using any base class, including open generics.
                var foundConcretes = assembliesToSearch
                    .Distinct()
                    .SelectMany(assembly => assembly.DefinedTypes)
                    .Where(foundType => !foundType.IsAbstract && foundType.Implements(baseType));

                foreach (var foundConcrete in foundConcretes)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(foundConcrete);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(foundConcrete);
                            break;
                        default:
                            services.AddTransient(foundConcrete);
                            break;
                    }
                }
            }
        }
    }
}
