using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VoidCore.Model.Configuration;
using Xunit;

namespace VoidCore.Test.Model.Configuration;

public class TypeServiceCollectionExtensionsTests
{
    private static readonly Assembly[] s_assemblies = [typeof(TypeServiceCollectionExtensionsTests).Assembly];

    [Theory]
    [InlineData(typeof(IMy<>))]
    [InlineData(typeof(MyAbstract<>))]
    public void Open_generic_base_types_register_closed_implementations_by_their_closed_base_type(Type openBaseType)
    {
        var services = new ServiceCollection();

        services.AddServicesFromAssembliesAsBaseType(ServiceLifetime.Singleton, [openBaseType], s_assemblies);

        // Registering a closed implementation against an open generic service type makes the provider throw.
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });

        var closedBaseType = openBaseType.MakeGenericType(typeof(string));
        Assert.Contains(provider.GetServices(closedBaseType), service => service is My);
    }

    [Fact]
    public void Non_generic_base_types_are_registered_as_themselves()
    {
        var services = new ServiceCollection();

        services.AddServicesFromAssembliesAsBaseType(ServiceLifetime.Transient, [typeof(IMy)], s_assemblies);

        using var provider = services.BuildServiceProvider();

        Assert.Contains(provider.GetServices<IMy>(), service => service is My);
    }

    [Fact]
    public void Unsupported_lifetimes_throw_NotSupportedException()
    {
        Assert.Throws<NotSupportedException>(() =>
            new ServiceCollection().AddServicesFromAssembliesAsBaseType((ServiceLifetime)42, [typeof(IMy)], s_assemblies));
    }
}
