using VoidCore.AspNet.Security;
using Xunit;

namespace VoidCore.Test.AspNet.Security
{
    public class XFrameOptionsOptionsBuilderTests
    {
        [Fact]
        public void EmptyOptionsIsDeny()
        {
            var builder = new XFrameOptionsOptionsBuilder();

            var header = new XFrameOptionsHeader(builder.Build());

            Assert.Equal("X-Frame-Options", header.Key);
            Assert.Equal("deny", header.Value);
        }

        [Fact]
        public void DenyOptions()
        {
            var builder = new XFrameOptionsOptionsBuilder();

            builder.Deny();

            var header = new XFrameOptionsHeader(builder.Build());

            Assert.Equal("X-Frame-Options", header.Key);
            Assert.Equal("deny", header.Value);
        }

        [Fact]
        public void SameOriginOptions()
        {
            var builder = new XFrameOptionsOptionsBuilder();

            builder.SameOrigin();

            var header = new XFrameOptionsHeader(builder.Build());

            Assert.Equal("X-Frame-Options", header.Key);
            Assert.Equal("sameorigin", header.Value);
        }

        [Fact]
        public void AllowFromOptions()
        {
            var builder = new XFrameOptionsOptionsBuilder();

            builder.AllowFrom("https://some.uri");

            var header = new XFrameOptionsHeader(builder.Build());

            Assert.Equal("X-Frame-Options", header.Key);
            Assert.Equal("allow-from https://some.uri", header.Value);
        }
    }
}
