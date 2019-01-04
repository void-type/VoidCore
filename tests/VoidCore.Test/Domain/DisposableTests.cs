using Moq;
using System;
using VoidCore.Domain.Functional;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class DisposableTests
    {
        [Fact]
        public void UsingReturnsExpectedValueAndDisposes()
        {
            var disposableMock = new Mock<IDisposableObject>();
            disposableMock.Setup(d => d.GetValue()).Returns("Hello World");
            disposableMock.Setup(d => d.Dispose());

            var value = Disposable.Using(
                () => disposableMock.Object,
                d => d.GetValue()
            );

            Assert.Equal("Hello World", value);
            disposableMock.Verify(d => d.Dispose(), Times.Once);
        }

        public interface IDisposableObject : IDisposable
        {
            string GetValue();
        }
    }
}
