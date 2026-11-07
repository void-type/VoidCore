using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VoidCore.AspNet.Security;
using Xunit;

namespace VoidCore.Test.AspNet.Security;

public class AntiforgeryLoggingTests
{
    [Fact]
    public async Task Failed_validation_is_logged_as_a_warning_and_still_throws()
    {
        var (antiforgery, logs) = CreateAntiforgery();

        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;
        context.Request.Path = "/api/things/1/upload";

        await Assert.ThrowsAsync<AntiforgeryValidationException>(() => antiforgery.ValidateRequestAsync(context));

        var entry = Assert.Single(logs.Entries);
        Assert.Equal(LogLevel.Warning, entry.Level);
        Assert.Contains("POST", entry.Message, StringComparison.Ordinal);
        Assert.Contains("/api/things/1/upload", entry.Message, StringComparison.Ordinal);
        Assert.Contains("required antiforgery cookie", entry.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Invalid_request_check_is_logged_as_a_warning()
    {
        var (antiforgery, logs) = CreateAntiforgery();

        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Post;

        Assert.False(await antiforgery.IsRequestValidAsync(context));

        Assert.Equal(LogLevel.Warning, Assert.Single(logs.Entries).Level);
    }

    [Fact]
    public async Task Safe_requests_are_not_logged()
    {
        var (antiforgery, logs) = CreateAntiforgery();

        var context = new DefaultHttpContext();
        context.Request.Method = HttpMethods.Get;

        Assert.True(await antiforgery.IsRequestValidAsync(context));
        Assert.Empty(logs.Entries);
    }

    private static (IAntiforgery Antiforgery, CapturingLoggerProvider Logs) CreateAntiforgery()
    {
        var logs = new CapturingLoggerProvider();
        var environment = Substitute.For<IHostEnvironment>();
        environment.EnvironmentName.Returns(Environments.Production);

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddProvider(logs));
        services.AddSpaSecurityServices(environment);

        var antiforgery = services.BuildServiceProvider().GetRequiredService<IAntiforgery>();

        return (antiforgery, logs);
    }

    private sealed class CapturingLoggerProvider : ILoggerProvider
    {
        public List<(LogLevel Level, string Message)> Entries { get; } = [];

        public ILogger CreateLogger(string categoryName) => new CapturingLogger(this, categoryName);

        public void Dispose() { }

        private sealed class CapturingLogger(CapturingLoggerProvider provider, string categoryName) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                // Only capture our own warnings, not the framework's Information-level noise.
                if (categoryName.EndsWith("LoggingAntiforgery", StringComparison.Ordinal))
                {
                    provider.Entries.Add((logLevel, formatter(state, exception)));
                }
            }
        }
    }
}
