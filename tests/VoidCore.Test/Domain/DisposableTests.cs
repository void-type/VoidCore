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
            DisposableObject disposable = null;

            var value = Disposable.Using(
                () => new DisposableObject(),
                d =>
                {
                    disposable = d;
                    return d.GetValue();
                }
            );

            Assert.Equal("Hello World", value);
            Assert.True(disposable.Disposed);
        }

        [Fact]
        public async Task UsingAsyncReturnsExpectedValueAndDisposes()
        {
            DisposableObject disposable = null;

            var value = await Disposable.UsingAsync(
                () => new DisposableObject(),
                d =>
                {
                    disposable = d;
                    return d.GetValueAsync();
                }
            );

            Assert.Equal("Hello World", value);
            Assert.True(disposable.Disposed);
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
