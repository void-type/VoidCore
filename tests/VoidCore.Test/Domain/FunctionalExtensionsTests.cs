using System.Threading.Tasks;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class FunctionalExtensionsTests
    {
        [Fact]
        public void MapReturnsMappedValue()
        {
            var t = new TestTransformerService();

            var actual = TestTransformerService.Start
                .Map(i => t.Transform(i, 1))
                .Map(i => t.Transform(i, 2));

            Assert.Equal("Hello World!!", actual);
        }

        [Fact]
        public async Task MapAsyncReturnsMappedValue()
        {
            var t = new TestTransformerService();

            var actual = await TestTransformerService.Start
                .MapAsync(i => t.TransformAsync(i, 1))
                .MapAsync(i => t.Transform(i, 2))
                .MapAsync(i => t.TransformAsync(i, 3));

            Assert.Equal("Hello World!!!", actual);
        }

        [Fact]
        public void TeeReturnsInput()
        {
            var p = new TestPerformerService();

            var actual = TestPerformerService.Start
                .Tee(a => p.Do(a, 1))
                .Tee(() => p.Go(2));

            Assert.Same("Hello World", actual);
        }

        [Fact]
        public async Task TeeAsyncReturnsInput()
        {
            var p = new TestPerformerService();

            var actual = await TestPerformerService.Start
                .TeeAsync(i => p.DoAsync(i, 1))
                .TeeAsync(() => p.GoAsync(2))
                .TeeAsync(i => p.DoAsync(i, 3))
                .TeeAsync(() => p.Go(4))
                .TeeAsync(i => p.Do(i, 5));

            Assert.Equal("Hello World", actual);

            var p2 = new TestPerformerService();

            var actual2 = await TestPerformerService.Start
                .TeeAsync(() => p2.GoAsync(1));

            Assert.Equal("Hello World", actual2);
        }
    }
}
