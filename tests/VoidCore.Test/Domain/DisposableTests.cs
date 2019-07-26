using System;
using System.Threading.Tasks;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class DisposableTests
    {
        [Fact]
        public void UsingReturnsExpectedValueAndDisposes()
        {
            var disposable = new DisposableObject();

            var value = Disposable.Using(
                () => disposable,
                d => d.GetValue()
            );

            Assert.Equal("Hello World", value);
            Assert.True(disposable.Disposed);

            disposable.Dispose();
        }

        [Fact]
        public async Task UsingAsyncReturnsExpectedValueAndDisposes()
        {
            var disposable = new DisposableObject();

            var value = await Disposable.UsingAsync(
                () => disposable,
                d => d.GetValueAsync()
            );

            Assert.Equal("Hello World", value);
            Assert.True(disposable.Disposed);

            disposable.Dispose();
        }

        private class DisposableObject : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                Disposed = true;
            }

            public string GetValue()
            {
                return "Hello World";
            }

            public async Task<string> GetValueAsync()
            {
                await Task.Delay(1);
                return "Hello World";
            }
        }
    }
}
