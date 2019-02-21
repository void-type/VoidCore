using Moq;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class FunctionalExtensionsTests
    {
        [Fact]
        public void MapStringArrayReturnsMappedValue()
        {
            var myStrings = new [] { "Hello", " World" };

            var actual = myStrings.Map(i => string.Join("", i));

            Assert.Equal("Hello World", actual);
        }

        [Fact]
        public void TeeStringArrayReturnsSameReferenceAsInput()
        {
            var myStrings = new [] { "Hello", " World" };

            var actual = myStrings.Tee(sa => sa[0] = "O");

            Assert.Same(myStrings, actual);
        }

        [Fact]
        public void TeeStringActionWithoutValue()
        {
            var myStrings = new [] { "Hello", " World" };

            var actionableMock = new Mock<IActionable>();
            actionableMock.Setup(a => a.Go());

            var actual = myStrings.Tee(actionableMock.Object.Go);

            actionableMock.Verify(a => a.Go(), Times.Once);

            Assert.Same(myStrings, actual);
        }

        public interface IActionable
        {
            void Go();
        }
    }
}
