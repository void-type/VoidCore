using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class EventTests
    {
        [Fact]
        public async Task Event_is_handled_with_no_validator_or_post_processors()
        {
            var result = await new TestEventOk()
                .Handle(new TestRequest());

            Assert.True(result.IsSuccess);
            Assert.Equal("success", result.Value.Name);
        }

        [Fact]
        public async Task Event_is_handled_with_no_post_processors_when_request_is_valid()
        {
            var validatorMock = MockIRequestValidatorOk();

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
        }

        [Fact]
        public async Task Event_is_handled_and_post_processors_run_with_no_validator()
        {
            var processorMock = MockIPostProcessor();

            var result = await new TestEventOk()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async Task Event_is_handled_and_post_processors_run_when_request_is_valid()
        {
            var validatorMock = MockIRequestValidatorOk();
            var processorMock = MockIPostProcessor();

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Event_is_not_handled_and_post_processors_run_when_request_is_invalid()
        {
            var validatorMock = MockIRequestValidatorFail();
            var processorMock = MockIPostProcessor();

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("request invalid", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async Task Post_processor_runs_when_event_fails()
        {
            var processorMock = MockIPostProcessor();

            var result = await new TestEventFail()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("event failed", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async Task Post_processor_abstract_calls_OnBoth_and_OnSuccess_when_result_ok()
        {
            var validatorMock = MockIRequestValidatorOk();
            var processorMock = MockPostProcessorAbstract();

            await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Protected().Verify("OnFailure", Times.Never(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IEnumerable<IFailure>>());
            processorMock.Protected().Verify("OnBoth", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult<TestResponse>>());
            processorMock.Protected().Verify("OnSuccess", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<TestResponse>());
        }

        [Fact]
        public async Task Post_processor_abstract_calls_OnBoth_and_OnFailure_when_result_fail()
        {
            var processorMock = MockPostProcessorAbstract();

            await new TestEventFail()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Protected().Verify("OnFailure", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IEnumerable<IFailure>>());
            processorMock.Protected().Verify("OnBoth", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult<TestResponse>>());
            processorMock.Protected().Verify("OnSuccess", Times.Never(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<TestResponse>());
        }

        [Fact]
        public async Task Post_processor_abstract_calls_OnBoth_and_OnFailure_when_request_invalid()
        {
            var validatorMock = MockIRequestValidatorFail();
            var processorMock = MockPostProcessorAbstract();

            await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Protected().Verify("OnFailure", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IEnumerable<IFailure>>());
            processorMock.Protected().Verify("OnBoth", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult<TestResponse>>());
            processorMock.Protected().Verify("OnSuccess", Times.Never(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<TestResponse>());
        }

        [Fact]
        public async Task Synchronous_event_is_handled_and_post_processors_run_when_request_is_valid()
        {
            var validatorMock = MockIRequestValidatorOk();
            var processorMock = MockIPostProcessor();

            var result = await new TestEventSyncOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task Synchronous_event_is_not_handled_and_post_processors_run_when_request_is_invalid()
        {
            var validatorMock = MockIRequestValidatorFail();
            var processorMock = MockIPostProcessor();

            var result = await new TestEventSyncOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("request invalid", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        public class TestRequest { }

        public class TestResponse
        {
            public string Name { get; set; }
        }

        private class TestEventFail : EventHandlerAbstract<TestRequest, TestResponse>
        {
            public override async Task<IResult<TestResponse>> Handle(TestRequest validRequest, CancellationToken cancellationToken = default)
            {
                await Task.Delay(1, cancellationToken);
                return Fail(new Failure("event failed"));
            }
        }

        private class TestEventOk : EventHandlerAbstract<TestRequest, TestResponse>
        {
            public override async Task<IResult<TestResponse>> Handle(TestRequest validRequest, CancellationToken cancellationToken = default)
            {
                await Task.Delay(1, cancellationToken);
                return Ok(new TestResponse { Name = "success" });
            }
        }

        private class TestEventSyncOk : EventHandlerSyncAbstract<TestRequest, TestResponse>
        {
            protected override IResult<TestResponse> HandleSync(TestRequest request)
            {
                return Ok(new TestResponse { Name = "success" });
            }
        }

        private static Mock<IRequestValidator<TestRequest>> MockIRequestValidatorOk()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Ok());
            return validatorMock;
        }

        private static Mock<IRequestValidator<TestRequest>> MockIRequestValidatorFail()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                .Returns(Result.Fail<TestResponse>(new Failure("request invalid")));
            return validatorMock;
        }

        private static Mock<IPostProcessor<TestRequest, TestResponse>> MockIPostProcessor()
        {
            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));
            return processorMock;
        }

        private static Mock<PostProcessorAbstract<TestRequest, TestResponse>> MockPostProcessorAbstract()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Protected().Setup("OnFailure", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IEnumerable<IFailure>>());
            processorMock.Protected().Setup("OnBoth", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult<TestResponse>>());
            processorMock.Protected().Setup("OnSuccess", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<TestResponse>());
            return processorMock;
        }
    }
}
