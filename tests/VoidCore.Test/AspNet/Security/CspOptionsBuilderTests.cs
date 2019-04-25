using System;
using VoidCore.AspNet.Security;
using Xunit;

namespace VoidCore.Test.AspNet.Security
{
    public class CspOptionsBuilderTests
    {
        [Fact]
        public void AllOptions()
        {
            var builder = new CspOptionsBuilder();

            builder.Defaults
                .AllowSelf();

            builder.Custom("customDirective")
                .AllowHash("sha256", "hash1")
                .AllowHash("sha256", "hash2");

            builder.Fonts
                .AllowAny();

            builder.Images
                .AllowNonce("nonce");

            builder.Media
                .AllowNone();

            builder.Scripts
                .AllowUnsafeEval();

            builder.Styles
                .AllowUnsafeInline()
                .Allow("data:");

            builder.SetReportUri("https://some.uri");

            var header = new CspHeader(builder.Build());

            Assert.Equal("Content-Security-Policy", header.Key);

            Assert.Contains("default-src 'self'; ", header.Value);
            Assert.Contains("customDirective 'sha256-hash1' 'sha256-hash2'; ", header.Value);
            Assert.Contains("font-src *; ", header.Value);
            Assert.Contains("img-src 'nonce-nonce'; ", header.Value);
            Assert.Contains("media-src 'none'; ", header.Value);
            Assert.Contains("script-src 'unsafe-eval'; ", header.Value);
            Assert.Contains("style-src 'unsafe-inline' data:; ", header.Value);
            Assert.Contains("report-uri https://some.uri; ", header.Value);

            builder.ReportOnly();
            header = new CspHeader(builder.Build());

            Assert.Equal("Content-Security-Policy-Report-Only", header.Key);
        }

        [Fact]
        public void EmptyOptions()
        {
            var builder = new CspOptionsBuilder();

            var header = new CspHeader(builder.Build());

            Assert.Equal("Content-Security-Policy", header.Key);

            Assert.Empty(header.Value);

            builder.ReportOnly();
            header = new CspHeader(builder.Build());

            Assert.Equal("Content-Security-Policy-Report-Only", header.Key);
        }

        [Fact]
        public void ReportOnlyWithUriOptions()
        {
            var builder = new CspOptionsBuilder();

            builder.ReportOnly("https://some.uri");

            builder.Scripts
                .AllowUnsafeEval();

            var header = new CspHeader(builder.Build());

            Assert.Equal("Content-Security-Policy-Report-Only", header.Key);
            Assert.Contains("script-src 'unsafe-eval'; ", header.Value);
            Assert.Contains("report-uri https://some.uri; ", header.Value);
        }

        [Fact]
        public void SettingMoreThanOneUriThrowInvalidOperationException()
        {
            var builder = new CspOptionsBuilder();

            builder.ReportOnly("https://some.uri");

            Assert.Throws<InvalidOperationException>(() => builder.SetReportUri("https://some.uri"));
        }
    }
}
