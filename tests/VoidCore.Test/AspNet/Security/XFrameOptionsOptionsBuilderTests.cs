using VoidCore.AspNet.Security;
using Xunit;

namespace VoidCore.Test.AspNet.Security
{
    public class XFrameOptionsOptionsBuilderTests
    {
        [Fact]
        public void Empty_options_is_deny()
        {
            var builder = new XFrameOptionsOptionsBuilder();

            var header = new XFrameOptionsHeader(builder.Build());

            Assert.Equal("X-Frame-Options", header.Key);
            Assert.Equal("deny", header.Value);
        }

        [Fact]
        public void Deny_option_is_deny()
        {
            var builder = new XFrameOptionsOptionsBuilder();

            builder.Deny();

            var header = new XFrameOptionsHeader(builder.Build());

            Assert.Equal("X-Frame-Options", header.Key);
            Assert.Equal("deny", header.Value);
        }

        [Fact]
        public void SameOrigin_option_is_sameorigin()
        {
            var builder = new XFrameOptionsOptionsBuilder();

            builder.SameOrigin();

            var header = new XFrameOptionsHeader(builder.Build());

            Assert.Equal("X-Frame-Options", header.Key);
            Assert.Equal("sameorigin", header.Value);
        }

        [Fact]
        public void AllowFrom_option_is_allow_with_uri()
        {
            var builder = new XFrameOptionsOptionsBuilder();

            builder.AllowFrom("https://some.uri");

            var header = new XFrameOptionsHeader(builder.Build());

            Assert.Equal("X-Frame-Options", header.Key);
            Assert.Equal("allow-from https://some.uri", header.Value);
        }
    }
}
