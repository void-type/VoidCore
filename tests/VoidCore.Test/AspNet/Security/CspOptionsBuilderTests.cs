﻿using System;
using VoidCore.AspNet.Security;
using Xunit;

namespace VoidCore.Test.AspNet.Security
{
    public class CspOptionsBuilderTests
    {
        [Fact]
        public void All_options_builds_correct_header()
        {
            var builder = new CspOptionsBuilder();

            builder.DefaultSources
                .AllowSelf();

            builder.ObjectSources
                .AllowNone();

            builder.FrameAncestors
                .AllowNone();

            builder.BaseUri
                .AllowSelf();

            builder.Custom("customDirective")
                .ForSources()
                .AllowHash("sha256", "hash1")
                .AllowHash("sha256", "hash2");

            builder.FontSources
                .AllowAny();

            builder.ImageSources
                .AllowNonce("nonce");

            builder.MediaSources
                .AllowNone();

            builder.ScriptSources
                .AllowUnsafeEval();

            builder.StyleSources
                .AllowUnsafeInline()
                .Allow("data:");

            builder.SetReportUri("https://some.uri");

            var header = new CspHeader(builder.Build());

            Assert.Equal("Content-Security-Policy", header.Key);

            Assert.Contains("default-src 'self';", header.Value);
            Assert.Contains("object-src 'none';", header.Value);
            Assert.Contains("frame-ancestors 'none';", header.Value);
            Assert.Contains("base-uri 'self';", header.Value);
            Assert.Contains("customDirective 'sha256-hash1' 'sha256-hash2';", header.Value);
            Assert.Contains("font-src *;", header.Value);
            Assert.Contains("img-src 'nonce-nonce';", header.Value);
            Assert.Contains("media-src 'none';", header.Value);
            Assert.Contains("script-src 'unsafe-eval';", header.Value);
            Assert.Contains("style-src 'unsafe-inline' data:;", header.Value);
            Assert.Contains("report-uri https://some.uri;", header.Value);

            builder.ReportOnly();
            header = new CspHeader(builder.Build());

            Assert.Equal("Content-Security-Policy-Report-Only", header.Key);
        }

        [Fact]
        public void Empty_options_builds_correct_header()
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
        public void ReportOnly_with_uri_options_builds_correct_header()
        {
            var builder = new CspOptionsBuilder();

            builder.ReportOnly("https://some.uri");

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
            var builder = new CspOptionsBuilder();

            builder.ReportOnly("https://some.uri");

            Assert.Throws<InvalidOperationException>(() => builder.SetReportUri("https://some.uri"));
        }
    }
}
