using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Configuration;

/// <summary>
/// Settings configuration extension methods for service collections.
/// </summary>
public static class TypeServiceCollectionExtensions
{
    /// <summary>
    /// Scan assemblies for domain event components and registers them with the DI container by implementation.
    /// This is a convenience method that has a cost of uniformity. All components are registered with the specified lifetime.
    /// You can override how a service gets registered by re-registering it under a different lifetime manually later in Startup.cs.
    /// </summary>
    /// <param name="services">This service collection</param>
    /// <param name="lifetime">The ServiceLifetime to register the services for.</param>
    /// <param name="assembliesToScan">An array of assemblies to scan for domain components in</param>
    public static void AddDomainEvents(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assembliesToScan)
    {
        var domainEventTypes = new[] {
                typeof (IEventHandler<,>),
                typeof (IRequestLogger<>),
                typeof (IRequestValidator<>),
                typeof (IPostProcessor<,>)};

        AddServicesFromAssemblies(services, lifetime, domainEventTypes, assembliesToScan);
    }

    /// <summary>
    /// Scan assemblies for implementations of classes and interfaces and registers them with the DI container by implementation.
    /// </summary>
    /// <param name="services">This service collection</param>
    /// <param name="lifetime">The ServiceLifetime to register the services for.</param>
    /// <param name="baseTypes">An array of base types or interfaces to register concrete implementations of</param>
    /// <param name="assembliesToScan">An array of assemblies to scan for implementations in</param>
    /// <exception cref="NotSupportedException">If an unsupported service lifetime is given</exception>
    public static void AddServicesFromAssemblies(this IServiceCollection services, ServiceLifetime lifetime, Type[] baseTypes, params Assembly[] assembliesToScan)
    {
        assembliesToScan.EnsureNotNullOrEmpty();

        foreach (var baseType in baseTypes)
        {
            foreach (var implementation in ScanAssembliesForTypes(assembliesToScan, baseType))
            {
                switch (lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(implementation);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(implementation);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(implementation);
                        break;
                    default:
                        throw new NotSupportedException($"Service lifetime of {lifetime} not supported.");
                }
            }
        }
    }

    /// <summary>
    /// Scan assemblies for implementations of classes and interfaces and registers them with the DI container by base type.
    /// </summary>
    /// <param name="services">This service collection</param>
    /// <param name="lifetime">The ServiceLifetime to register the services for.</param>
    /// <param name="baseTypes">An array of base types or interfaces to register concrete implementations of</param>
    /// <param name="assembliesToScan">An array of assemblies to scan for implementations in</param>
    /// <exception cref="NotSupportedException">If an unsupported service lifetime is given</exception>
    public static void AddServicesFromAssembliesAsBaseType(this IServiceCollection services, ServiceLifetime lifetime, Type[] baseTypes, params Assembly[] assembliesToScan)
    {
        assembliesToScan.EnsureNotNullOrEmpty();

        foreach (var baseType in baseTypes)
        {
            foreach (var implementation in ScanAssembliesForTypes(assembliesToScan, baseType))
            {
                foreach (var serviceType in GetServiceTypes(implementation, baseType))
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(serviceType, implementation);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(serviceType, implementation);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(serviceType, implementation);
                            break;
                        default:
                            throw new NotSupportedException($"Service lifetime of {lifetime} not supported.");
                    }
                }
            }
        }
    }

    // A closed implementation can only be registered against the closed form of an open generic base type, for example MyHandler against IHandler<A, B> and not IHandler<,>.
    private static IEnumerable<Type> GetServiceTypes(TypeInfo implementation, Type baseType)
    {
        // An open generic implementation can only be registered against an open generic service type.
        if (implementation.IsGenericTypeDefinition)
        {
            return baseType.IsGenericTypeDefinition ? [baseType] : [];
        }

        if (!baseType.IsGenericTypeDefinition)
        {
            return [baseType];
        }

        if (baseType.IsInterface)
        {
            return implementation
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseType);
        }

        var closedBaseTypes = new List<Type>();

        for (var type = implementation.BaseType; type is not null; type = type.BaseType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == baseType)
            {
                closedBaseTypes.Add(type);
            }
        }

        return closedBaseTypes;
    }

    private static IEnumerable<TypeInfo> ScanAssembliesForTypes(Assembly[] assembliesToScan, Type baseType)
    {
        return assembliesToScan
            .Distinct()
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(foundType => !foundType.IsAbstract && foundType.Implements(baseType));
    }
}
