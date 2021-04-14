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
        /// Search assemblies for domain pipeline components and registers them with the DI container by concrete name.
        /// This is a convenience method that has a cost of uniformity. All components are registered with the specified lifetime.
        /// You can override how a service gets registered by re-registering it under a different lifetime manually later in Startup.cs.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="lifetime">The ServiceLifetime to register the services for.</param>
        /// <param name="assembliesToSearch">An array of assemblies to search for domain components in</param>
        public static void FindAndRegisterDomainEvents(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assembliesToSearch)
        {
            var domainEventTypes = new[] {
                typeof (IEventHandler<,>),
                typeof (IRequestLogger<>),
                typeof (IRequestValidator<>),
                typeof (IPostProcessor<,>)};

            FindAndRegisterByInterface(services, lifetime, domainEventTypes, assembliesToSearch);
        }

        /// <summary>
        /// Search assemblies for implementations and registers them with the DI container by concrete name.
        /// </summary>
        /// <param name="services">This service collection</param>
        /// <param name="lifetime">The ServiceLifetime to register the services for.</param>
        /// <param name="typesToRegister">An array of base types or interfaces to register concrete implementations of</param>
        /// <param name="assembliesToSearch">An array of assemblies to search for domain components in</param>
        public static void FindAndRegisterByInterface(this IServiceCollection services, ServiceLifetime lifetime, Type[] typesToRegister, params Assembly[] assembliesToSearch)
        {
            assembliesToSearch.EnsureNotNullOrEmpty(nameof(assembliesToSearch));

            foreach (var desiredType in typesToRegister)
            {
                // DI can register concrete types using any base class, including open generics.
                var matchingConcretes = assembliesToSearch
                    .Distinct()
                    .SelectMany(assembly => assembly.DefinedTypes)
                    .Where(foundType => !foundType.IsAbstract && foundType.Inherits(desiredType));

                foreach (var matchingConcrete in matchingConcretes)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(matchingConcrete);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(matchingConcrete);
                            break;
                        default:
                            services.AddTransient(matchingConcrete);
                            break;
                    }
                }
            }
        }
    }
}
