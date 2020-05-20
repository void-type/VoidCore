using System.Linq;
using System.Threading.Tasks;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class ResultExtensionsTeeTests
    {
        [Fact]
        public void TeeOnSuccess_runs_when_result_is_success()
        {
            var tick = 1;
            var okResult = Result.Ok();

            okResult
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public void TeeOnSuccess_doesnt_run_when_result_is_failed()
        {
            var tick = 1;
            var failResult = Result.Fail(new Failure("oops"));

            failResult
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(() => tick++);

            Assert.Equal(1, tick);
        }

        [Fact]
        public void TypedTeeOnSuccess_runs_when_result_is_success()
        {
            var tick = 1;
            var okResult = Result.Ok(2);

            okResult
                .TeeOnSuccess(r => tick += r)
                .TeeOnSuccess(r => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(r => tick += r);

            Assert.Equal(7, tick);
        }

        [Fact]
        public void TypedTeeOnSuccess_doesnt_run_when_result_is_failed()
        {
            var tick = 1;
            var failResult = Result.Fail<int>(new Failure("oops"));

            failResult
                .TeeOnSuccess(r => tick += r)
                .TeeOnSuccess(r => tick++)
                .TeeOnSuccess(() => tick++)
                .TeeOnSuccess(r => tick += r);

            Assert.Equal(1, tick);
        }

        [Fact]
        public void TeeOnFailure_runs_when_result_is_failed()
        {
            var tick = 1;
            var failResult = Result.Fail(new Failure("oops"));

            failResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(failures => Assert.Single(failures))
                .TeeOnFailure(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public void TeeOnFailure_doesnt_run_when_result_is_success()
        {
            var tick = 1;
            var okResult = Result.Ok();

            okResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(failures => Assert.Single(failures))
                .TeeOnFailure(() => tick++);

            Assert.Equal(1, tick);
        }

        [Fact]
        public void TypedTeeOnFailure_runs_when_result_is_failed()
        {
            var tick = 1;
            var failResult = Result.Fail<int>(new Failure("oops"));

            failResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(failures => Assert.Single(failures))
                .TeeOnFailure(() => tick++);

            Assert.Equal(4, tick);
        }

        [Fact]
        public void TypedTeeOnFailure_doesnt_run_when_result_is_success()
        {
            var tick = 1;
            var okResult = Result.Ok(2);

            okResult
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(() => tick++)
                .TeeOnFailure(failures => Assert.Single(failures))
                .TeeOnFailure(() => tick++);

            Assert.Equal(1, tick);
        }

        [Fact]
        public async Task TeeAsync_tests()
        {
            var p = new TestPerformerService();

            var newOkResult = await Result.Ok()
                .TeeOnSuccessAsync(() => p.DoAsync(1))
                .TeeOnFailureAsync(() => p.DoAsync(1))
                .TeeOnFailureAsync(failures => Assert.Single(failures))
                .TeeOnFailureAsync(async failures => await Task.FromResult(Assert.Single(failures)))
                .TeeOnFailureAsync(() => p.Do(2))
                .TeeOnSuccessAsync(() => p.DoAsync(2))
                .TeeOnFailureAsync(() => p.Do(3))
                .TeeOnSuccessAsync(() => p.Do(3))
                .TeeOnFailureAsync(() => p.DoAsync(4))
                .TeeOnSuccessAsync(() => p.DoAsync(4))
                .TeeOnFailureAsync(() => p.DoAsync(5))
                .TeeOnSuccessAsync(() => p.DoAsync(5));

            Assert.True(newOkResult.IsSuccess);

            p.Reset();

            var newFailResult = await Result.Fail(new Failure("oops"))
                .TeeOnSuccessAsync(() => p.DoAsync(1))
                .TeeOnFailureAsync(() => p.DoAsync(1))
                .TeeOnFailureAsync(failures => Assert.Single(failures))
                .TeeOnFailureAsync(async failures => await Task.FromResult(Assert.Single(failures)))
                .TeeOnFailureAsync(() => p.Do(2))
                .TeeOnSuccessAsync(() => p.DoAsync(2))
                .TeeOnFailureAsync(() => p.Do(3))
                .TeeOnSuccessAsync(() => p.Do(3))
                .TeeOnFailureAsync(() => p.DoAsync(4))
                .TeeOnSuccessAsync(() => p.DoAsync(4))
                .TeeOnFailureAsync(() => p.DoAsync(5))
                .TeeOnSuccessAsync(() => p.DoAsync(5));

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("oops", newFailResult.Failures.First().Message);
        }

        [Fact]
        public async Task TypedTeeAsync_tests()
        {
            var p = new TestPerformerService();

            var newOkResult = await Result.Ok(string.Empty)
                .TeeOnSuccessAsync(r => p.DoAsync(1))
                .TeeOnFailureAsync(() => p.DoAsync(1))
                .TeeOnFailureAsync(failures => Assert.Single(failures))
                .TeeOnFailureAsync(async failures => await Task.FromResult(Assert.Single(failures)))
                .TeeOnFailureAsync(() => p.Do(2))
                .TeeOnSuccessAsync(() => p.Do(2))
                .TeeOnFailureAsync(() => p.Do(3))
                .TeeOnSuccessAsync(r => p.Do(3))
                .TeeOnFailureAsync(() => p.DoAsync(4))
                .TeeOnSuccessAsync(() => p.DoAsync(4))
                .TeeOnFailureAsync(() => p.DoAsync(5))
                .TeeOnSuccessAsync(r => p.DoAsync(5));

            Assert.True(newOkResult.IsSuccess);

            p.Reset();

            var newFailResult = await Result.Fail<int>(new Failure("oops"))
                .TeeOnSuccessAsync(r => p.DoAsync(1))
                .TeeOnFailureAsync(() => p.DoAsync(1))
                .TeeOnFailureAsync(failures => Assert.Single(failures))
                .TeeOnFailureAsync(async failures => await Task.FromResult(Assert.Single(failures)))
                .TeeOnFailureAsync(() => p.Do(2))
                .TeeOnSuccessAsync(() => p.Do(2))
                .TeeOnFailureAsync(() => p.Do(3))
                .TeeOnSuccessAsync(r => p.Do(3))
                .TeeOnFailureAsync(() => p.DoAsync(4))
                .TeeOnSuccessAsync(() => p.DoAsync(4))
                .TeeOnFailureAsync(() => p.DoAsync(5))
                .TeeOnSuccessAsync(r => p.DoAsync(5));

            Assert.True(newFailResult.IsFailed);
            Assert.Equal("oops", newFailResult.Failures.First().Message);
        }
    }
}
