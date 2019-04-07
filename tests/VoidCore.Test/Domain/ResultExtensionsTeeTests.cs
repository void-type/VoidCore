using System.Linq;
using System.Threading.Tasks;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class ResultExtensionsTeeTests
    {
        [Fact]
        public void TeeOnSuccessTests()
        {
            var tick = 1;
            var okResult = Result.Ok();
            var failResult = Result.Fail(new Failure("oops"));

            okResult
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++);

            Assert.Equal(4, tick);

            failResult
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public void TypedTeeOnSuccessTests()
        {
            var tick = 1;
            var okResult = Result.Ok(2);
            var failResult = Result.Fail<int>(new Failure("oops"));

            okResult
                .TeeOnSuccess(r => tick += r)
                .TeeOnSuccess(r => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(r => tick += r);

            Assert.Equal(7, tick);

            failResult
                .TeeOnSuccess(r => tick += r)
                .TeeOnSuccess(r => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(r => tick += r);

            Assert.Equal(7, tick);
        }

        [Fact]
        public void TeeOnFailureTests()
        {
            var tick = 1;
            var okResult = Result.Ok();
            var failResult = Result.Fail(new Failure("oops"));

            okResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(r => Assert.Single(r.Failures))
                .TeeOnFailure(() => tick++);

            Assert.Equal(1, tick);

            failResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(r => Assert.Single(r.Failures))
                .TeeOnFailure(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public void TypedTeeOnFailureTests()
        {
            var tick = 1;
            var okResult = Result.Ok(2);
            var failResult = Result.Fail<int>(new Failure("oops"));

            okResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(r => Assert.Single(r.Failures))
                .TeeOnFailure(() => tick++);

            Assert.Equal(1, tick);

            failResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(r => Assert.Single(r.Failures))
                .TeeOnFailure(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public async Task TeeAsyncTests()
        {
            var p = new TestPerformerService();

            var newOkResult = await Result.Ok()
                .TeeOnSuccessAsync(() => p.GoAsync(1))
                .TeeOnFailureAsync(() => p.GoAsync(1))
                .TeeOnFailureAsync(r => Assert.Single(r.Failures))
                .TeeOnFailureAsync(async r => await Task.FromResult(Assert.Single(r.Failures)))
                .TeeOnFailureAsync(() => p.Go(2))
                .TeeOnSuccessAsync(() => p.GoAsync(2))
                .TeeOnFailureAsync(() => p.Go(3))
                .TeeOnSuccessAsync(() => p.Go(3))
                .TeeOnFailureAsync(() => p.GoAsync(4))
                .TeeOnSuccessAsync(() => p.GoAsync(4))
                .TeeOnFailureAsync(() => p.GoAsync(5))
                .TeeOnSuccessAsync(() => p.GoAsync(5));

            Assert.True(newOkResult.IsSuccess);

            p = new TestPerformerService();

            var newFailResult = await Result.Fail(new Failure("oops"))
                .TeeOnSuccessAsync(() => p.GoAsync(1))
                .TeeOnFailureAsync(() => p.GoAsync(1))
                .TeeOnFailureAsync(r => Assert.Single(r.Failures))
                .TeeOnFailureAsync(async r => await Task.FromResult(Assert.Single(r.Failures)))
                .TeeOnFailureAsync(() => p.Go(2))
                .TeeOnSuccessAsync(() => p.GoAsync(2))
                .TeeOnFailureAsync(() => p.Go(3))
                .TeeOnSuccessAsync(() => p.Go(3))
                .TeeOnFailureAsync(() => p.GoAsync(4))
                .TeeOnSuccessAsync(() => p.GoAsync(4))
                .TeeOnFailureAsync(() => p.GoAsync(5))
                .TeeOnSuccessAsync(() => p.GoAsync(5));

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("oops", newFailResult.Failures.First().Message);
        }

        [Fact]
        public async Task TypedTeeAsyncTests()
        {
            var p = new TestPerformerService();

            var newOkResult = await Result.Ok(string.Empty)
                .TeeOnSuccessAsync(r => p.GoAsync(1))
                .TeeOnFailureAsync(() => p.GoAsync(1))
                .TeeOnFailureAsync(r => Assert.Single(r.Failures))
                .TeeOnFailureAsync(async r => await Task.FromResult(Assert.Single(r.Failures)))
                .TeeOnFailureAsync(() => p.Go(2))
                .TeeOnSuccessAsync(() => p.Go(2))
                .TeeOnFailureAsync(() => p.Go(3))
                .TeeOnSuccessAsync(r => p.Go(3))
                .TeeOnFailureAsync(() => p.GoAsync(4))
                .TeeOnSuccessAsync(() => p.GoAsync(4))
                .TeeOnFailureAsync(() => p.GoAsync(5))
                .TeeOnSuccessAsync(r => p.GoAsync(5));

            Assert.True(newOkResult.IsSuccess);

            p = new TestPerformerService();

            var newFailResult = await Result.Fail<int>(new Failure("oops"))
                .TeeOnSuccessAsync(r => p.GoAsync(1))
                .TeeOnFailureAsync(() => p.GoAsync(1))
                .TeeOnFailureAsync(r => Assert.Single(r.Failures))
                .TeeOnFailureAsync(async r => await Task.FromResult(Assert.Single(r.Failures)))
                .TeeOnFailureAsync(() => p.Go(2))
                .TeeOnSuccessAsync(() => p.Go(2))
                .TeeOnFailureAsync(() => p.Go(3))
                .TeeOnSuccessAsync(r => p.Go(3))
                .TeeOnFailureAsync(() => p.GoAsync(4))
                .TeeOnSuccessAsync(() => p.GoAsync(4))
                .TeeOnFailureAsync(() => p.GoAsync(5))
                .TeeOnSuccessAsync(r => p.GoAsync(5));

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("oops", newFailResult.Failures.First().Message);
        }
    }
}
