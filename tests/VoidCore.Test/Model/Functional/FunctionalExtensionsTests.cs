using System.Threading.Tasks;
using VoidCore.Model.Functional;
using Xunit;

namespace VoidCore.Test.Model.Functional
{
    public class FunctionalExtensionsTests
    {
        [Fact]
        public void Map_runs_function_against_input_and_returns_output()
        {
            var t = new TestTransformerService();

            var actual = TestTransformerService.Start
                .Map(i => t.Transform(i, 1))
                .Map(i => t.Transform(i, 2));

            Assert.Equal("Hello World!!", actual);
        }

        [Fact]
        public async Task MapAsync_awaits_as_needed()
        {
            var t = new TestTransformerService();

            var actual = await TestTransformerService.Start
                .MapAsync(i => t.TransformAsync(i, 1))
                .MapAsync(i => t.Transform(i, 2))
                .MapAsync(i => t.TransformAsync(i, 3));

            Assert.Equal("Hello World!!!", actual);
        }

        [Fact]
        public void Tee_runs_function_and_returns_input()
        {
            var p = new TestPerformerService();

            var actual = TestPerformerService.Start
                .Tee(a => p.Do(1))
                .Tee(() => p.Do(2));

            Assert.Same("Hello World", actual);
        }

        [Fact]
        public async Task TeeAsync_awaits_as_needed_and_runs_functions_in_order()
        {
            var p = new TestPerformerService();

            var actual = await TestPerformerService.Start
                .TeeAsync(i => p.DoAsync(1))
                .TeeAsync(() => p.DoAsync(2))
                .TeeAsync(() => p.Do(3))
                .TeeAsync(i => p.Do(4));

            Assert.Equal("Hello World", actual);

            p.Reset();

            var actual2 = await TestPerformerService.Start
                .TeeAsync(() => p.DoAsync(1));

            Assert.Equal("Hello World", actual2);
        }
    }
}
