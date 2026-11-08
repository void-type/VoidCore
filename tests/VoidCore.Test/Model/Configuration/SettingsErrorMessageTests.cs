using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.Model.Configuration;
using VoidCore.Model.Functional;
using Xunit;

namespace VoidCore.Test.Model.Configuration;

public class SettingsErrorMessageTests
{
    [Fact]
    public void Missing_settings_section_names_the_type_and_section()
    {
        var configuration = new ConfigurationBuilder().Build();

        var ex = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddSettingsSingleton<DemoSettings>(configuration));

        Assert.Contains("DemoSettings", ex.Message, StringComparison.Ordinal);
        Assert.Contains("'Demo' section", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Missing_root_settings_says_so()
    {
        var configuration = new ConfigurationBuilder().Build();

        var ex = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddSettingsSingleton<DemoSettings>(configuration, true));

        Assert.Contains("root of the configuration", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Missing_connection_string_names_the_connection_string()
    {
        var configuration = new ConfigurationBuilder().Build();

        var ex = Assert.Throws<ArgumentNullException>(() => configuration.GetRequiredConnectionString("Reports"));

        Assert.Contains("'Reports'", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Tee_overloads_throw_ArgumentNullException_for_a_null_action()
    {
        Assert.Throws<ArgumentNullException>(() => 1.Tee((Action)null!));
        Assert.Throws<ArgumentNullException>(() => Task.FromResult(1).TeeAsync((Action)null!).GetAwaiter().GetResult());
        Assert.Throws<ArgumentNullException>(() => 1.TeeAsync((Func<Task>)null!).GetAwaiter().GetResult());
        Assert.Throws<ArgumentNullException>(() => Task.FromResult(1).TeeAsync((Func<Task>)null!).GetAwaiter().GetResult());
        Assert.Throws<ArgumentNullException>(() => Task.FromResult(1).TeeAsync((Func<int, Task>)null!).GetAwaiter().GetResult());
        Assert.Throws<ArgumentNullException>(() => ((Task<int>)null!).TeeAsync((Action)(() => { })).GetAwaiter().GetResult());
        Assert.Throws<ArgumentNullException>(() => ((Task<int>)null!).TeeAsync((Func<Task>)(() => Task.CompletedTask)).GetAwaiter().GetResult());
        Assert.Throws<ArgumentNullException>(() => ((Task<int>)null!).TeeAsync((Func<int, Task>)(_ => Task.CompletedTask)).GetAwaiter().GetResult());
    }

    private sealed class DemoSettings { }
}
