using VoidCore.AspNet.Security;
using Xunit;

namespace VoidCore.Test.AspNet.Security;

public class CspOptionsBuilderTests
{
    [Fact]
    public void All_options_builds_correct_header()
    {
        var builder = new CspOptionsBuilder("myNonce=");

        builder.FrameAncestors
            .AllowNone();

        builder.BaseUri
            .AllowSelf();

        builder.DefaultSources
            .AllowSelf()
            .AllowNonce();

        builder.FontSources
            .AllowAny();

        builder.ImageSources
            .AllowNonce("myOtherNonce");

        builder.MediaSources
            .AllowNone();

        builder.ObjectSources
            .AllowNone();

        builder.ScriptSources
            .AllowUnsafeEval();

        builder.StyleSources
            .AllowUnsafeInline()
            .Allow("data:");

        builder.Custom("customDirective")
            .ForSources("myNonce=")
            .AllowHash("sha256", "hash1")
            .AllowHash("sha256", "hash2")
            .AllowNonce();

#pragma warning disable CS0618
        builder.SetReportUri("https://some.uri");
#pragma warning restore CS0618

        var header = new CspHeader(builder.Build());

        Assert.Equal("Content-Security-Policy", header.Key);

        Assert.Contains("default-src 'self' 'nonce-myNonce=';", header.Value);
        Assert.Contains("object-src 'none';", header.Value);
        Assert.Contains("frame-ancestors 'none';", header.Value);
        Assert.Contains("base-uri 'self';", header.Value);
        Assert.Contains("customDirective 'sha256-hash1' 'sha256-hash2' 'nonce-myNonce=';", header.Value);
        Assert.Contains("font-src *;", header.Value);
        Assert.Contains("img-src 'nonce-myOtherNonce';", header.Value);
        Assert.Contains("media-src 'none';", header.Value);
        Assert.Contains("script-src 'unsafe-eval';", header.Value);
        Assert.Contains("style-src 'unsafe-inline' data:;", header.Value);
        Assert.Contains("report-uri https://some.uri;", header.Value);

#pragma warning disable CS0618
        builder.ReportOnly();
#pragma warning restore CS0618

        header = new CspHeader(builder.Build());

        Assert.Equal("Content-Security-Policy-Report-Only", header.Key);
    }

    [Fact]
    public void Empty_options_builds_correct_header()
    {
        var builder = new CspOptionsBuilder("myNonce=");

        var header = new CspHeader(builder.Build());

        Assert.Equal("Content-Security-Policy", header.Key);

        Assert.Empty(header.Value);

#pragma warning disable CS0618
        builder.ReportOnly();
#pragma warning restore CS0618

        header = new CspHeader(builder.Build());

        Assert.Equal("Content-Security-Policy-Report-Only", header.Key);
    }

    [Fact]
    public void ReportOnly_with_uri_options_builds_correct_header()
    {
        var builder = new CspOptionsBuilder("myNonce=");

#pragma warning disable CS0618
        builder.ReportOnly("https://some.uri");
#pragma warning restore CS0618

        builder.ScriptSources
            .AllowUnsafeEval();

        var header = new CspHeader(builder.Build());

        Assert.Equal("Content-Security-Policy-Report-Only", header.Key);
        Assert.Contains("script-src 'unsafe-eval';", header.Value);
        Assert.Contains("report-uri https://some.uri;", header.Value);
    }

    [Fact]
    public void Setting_more_than_one_uri_throws_InvalidOperationException()
    {
        var builder = new CspOptionsBuilder("my7nonce");

#pragma warning disable CS0618
        builder.ReportOnly("https://some.uri");
#pragma warning restore CS0618

#pragma warning disable CS0618
        Assert.Throws<InvalidOperationException>(() => builder.SetReportUri("https://some.uri"));
#pragma warning restore CS0618
    }
}
