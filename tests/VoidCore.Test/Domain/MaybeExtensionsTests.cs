using System.Linq;
using System.Threading.Tasks;
using VoidCore.Domain;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class MaybeExtensionsTests
    {
        [Fact]
        public void ToResult_from_Maybe_without_value_is_failed()
        {
            var maybe = Maybe.None<string>();
            var result = maybe.ToResult(new Failure("no value", "uiField"));

            Assert.True(result.IsFailed);
            var failure = result.Failures.Single();
            Assert.Equal("no value", failure.Message);
            Assert.Equal("uiField", failure.UiHandle);
        }

        [Fact]
        public void ToResult_from_Maybe_with_value_is_success()
        {
            var result = Maybe.From("some value").ToResult(new Failure("no value"));

            Assert.True(result.IsSuccess);
            Assert.Equal("some value", result.Value);
        }

        [Fact]
        public async Task ToResultAsync_from_Maybe_without_value_is_failed()
        {
            var maybeTask = Task.FromResult(Maybe.None<string>());

            var result = await maybeTask.ToResultAsync(new Failure("no value", "uiField"));

            Assert.True(result.IsFailed);
            var failure = result.Failures.Single();
            Assert.Equal("no value", failure.Message);
            Assert.Equal("uiField", failure.UiHandle);
        }

        [Fact]
        public async Task ToResultAsync_from_Maybe_with_value_is_success()
        {
            var maybeTask = Task.FromResult(Maybe.From("some value"));

            var result = await maybeTask.ToResultAsync(new Failure("no value", "uiField"));

            Assert.True(result.IsSuccess);
            Assert.Equal("some value", result.Value);
        }

        [Fact]
        public void Select_from_none_returns_none()
        {
            var maybe = Maybe.None<string>()
                .Select(v => 2);

            Assert.True(maybe is Maybe<int>);
            Assert.True(maybe.HasNoValue);
        }

        [Fact]
        public async Task SelectAsync_from_none_returns_none()
        {
            var t = new TestTransformerService();

            var maybe = await Maybe.None<string>()
                .SelectAsync(a => t.TransformAsync(a, 1))
                .SelectAsync(a => t.Transform(a, 2))
                .SelectAsync(a => t.TransformAsync(a, 3));

            Assert.True(maybe.HasNoValue);
        }

        [Fact]
        public void Select_from_maybe_with_value_returns_transformation()
        {
            var maybe = Maybe.From("some value")
                .Select(v => 2);

            Assert.True(maybe is Maybe<int>);
            Assert.Equal(2, maybe.Value);
        }

        [Fact]
        public async Task SelectAsync_from_maybe_with_value_returns_transformation()
        {
            var t = new TestTransformerService();

            var maybe = await Maybe.From(t.Start)
                .SelectAsync(a => t.TransformAsync(a, 1))
                .SelectAsync(a => t.Transform(a, 2))
                .SelectAsync(a => t.TransformAsync(a, 3));

            Assert.True(maybe.HasValue);
            Assert.Equal("Hello World!!!", maybe.Value);
        }

        [Fact]
        public void Then_from_none_returns_none()
        {
            var maybe = Maybe.None<string>()
                .Then(v => Maybe.From(2));

            Assert.True(maybe is Maybe<int>);
            Assert.True(maybe.HasNoValue);
        }

        [Fact]
        public async Task ThenAsync_from_none_returns_none()
        {
            var t = new TestTransformerService();

            var maybe = await Maybe.None<string>()
                .ThenAsync(a => t.TransformMaybeAsync(a, 1))
                .ThenAsync(a => t.TransformMaybe(a, 2))
                .ThenAsync(a => t.TransformMaybeAsync(a, 3));

            Assert.True(maybe.HasNoValue);
        }

        [Fact]
        public void Then_from_Maybe_with_value_returns_new_maybe()
        {
            var maybe = Maybe.From("some value")
                .Then(v => Maybe.From(2));

            Assert.True(maybe is Maybe<int>);
            Assert.Equal(2, maybe.Value);
        }

        [Fact]
        public async Task ThenAsync_from_Maybe_with_value_returns_new_maybe()
        {
            var t = new TestTransformerService();

            var maybe = await Maybe.From(t.Start)
                .ThenAsync(a => t.TransformMaybeAsync(a, 1))
                .ThenAsync(a => t.TransformMaybe(a, 2))
                .ThenAsync(a => t.TransformMaybeAsync(a, 3));

            Assert.True(maybe.HasValue);
            Assert.Equal("Hello World!!!", maybe.Value);
        }

        [Fact]
        public void Unwrap_none_returns_default()
        {
            var maybe = Maybe.None<string>();

            var valueNoDefault = maybe.Unwrap();
            Assert.Null(valueNoDefault);

            var valueWithDefault = maybe.Unwrap("default value");
            Assert.Equal("default value", valueWithDefault);

            var valueWithDefaultFactory = maybe.Unwrap(() => "default value");
            Assert.Equal("default value", valueWithDefaultFactory);
        }

        [Fact]
        public async Task UnwrapAsync_none_returns_default()
        {
            var maybe = Task.FromResult(Maybe.None<string>());

            var valueNoDefault = await maybe.UnwrapAsync();
            Assert.Null(valueNoDefault);

            var valueWithDefault = await maybe.UnwrapAsync("default value");
            Assert.Equal("default value", valueWithDefault);

            var valueWithDefaultFactory = await maybe.UnwrapAsync(() => "default value");
            Assert.Equal("default value", valueWithDefaultFactory);
        }

        [Fact]
        public void Unwrap_with_value_returns_value()
        {
            Maybe<string> maybe = "some value";

            var valueNoDefault = maybe.Unwrap();
            Assert.Equal("some value", valueNoDefault);

            var valueWithDefault = maybe.Unwrap("default value");
            Assert.Equal("some value", valueWithDefault);

            var valueWithDefaultFactory = maybe.Unwrap(() => "default value");
            Assert.Equal("some value", valueWithDefaultFactory);
        }

        [Fact]
        public async Task UnwrapAsync_with_value_returns_value()
        {
            var maybe = Task.FromResult(Maybe.From("some value"));

            var valueNoDefault = await maybe.UnwrapAsync();
            Assert.Equal("some value", valueNoDefault);

            var valueWithDefault = await maybe.UnwrapAsync("default value");
            Assert.Equal("some value", valueWithDefault);

            var valueWithDefaultFactory = await maybe.UnwrapAsync(() => "default value");
            Assert.Equal("some value", valueWithDefaultFactory);
        }

        [Fact]
        public void Where_with_value_and_false_predicate_returns_none()
        {
            Maybe<string> maybe = "some value";
            var queried = maybe.Where(v => false);

            Assert.True(queried.HasNoValue);
        }

        [Fact]
        public void Where_without_value_and_true_predicate_returns_none()
        {
            var maybe = Maybe.None<string>();
            var queried = maybe.Where(v => true);

            Assert.True(queried.HasNoValue);
        }

        [Fact]
        public void Where_with_value_and_true_predicate_returns_maybe()
        {
            Maybe<string> maybe = "some value";
            var queried = maybe.Where(v => true);

            Assert.True(queried.HasValue);
            Assert.Equal("some value", queried.Value);
        }

        [Fact]
        public async Task WhereAsync_with_value_and_false_predicate_returns_none()
        {
            var maybe = Task.FromResult(Maybe.From("some value"));
            var queried = await maybe.WhereAsync(v => false);

            Assert.True(queried.HasNoValue);
        }

        [Fact]
        public async Task WhereAsync_without_value_and_true_predicate_returns_none()
        {
            var maybe = Task.FromResult(Maybe.None<string>());
            var queried = await maybe.WhereAsync(v => true);

            Assert.True(queried.HasNoValue);
        }

        [Fact]
        public async Task WhereAsync_with_value_and_true_predicate_returns_maybe()
        {
            var maybe = Task.FromResult(Maybe.From("some value"));
            var queried = await maybe.WhereAsync(v => true);

            Assert.True(queried.HasValue);
            Assert.Equal("some value", queried.Value);
        }
    }
}
