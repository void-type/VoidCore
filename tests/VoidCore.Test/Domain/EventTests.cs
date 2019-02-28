using Moq;
using Moq.Protected;
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
        public async Task EventHandledAndPostProcessorsRunWhenRequestValid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                .Returns(Result.Ok());

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Exactly(2));
        }

        [Fact]
        public async Task EventHandledNoPostProcessorWhenRequestValid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                .Returns(Result.Ok());

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
        }

        [Fact]
        public async Task EventHandledNoValidatorOrPostProcessor()
        {
            var domainEvent = new TestEventOk();

            var result = await domainEvent.Handle(new TestRequest());

            Assert.True(result.IsSuccess);
            Assert.Equal("success", result.Value.Name);
        }

        [Fact]
        public async Task EventHandledNoValidatorWithPostProcessor()
        {
            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            var result = await new TestEventOk()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async Task EventNotHandledAndPostProcessorRunsWhenRequestInvalid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                .Returns(Result.Fail<TestResponse>(new Failure("request invalid")));

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("request invalid", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async Task EventSyncHandledAndPostProcessorsRunWhenRequestValid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                .Returns(Result.Ok());

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            var result = await new TestEventSyncOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task EventSyncNotHandledAndPostProcessorRunsWhenRequestInvalid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                .Returns(Result.Fail<TestResponse>(new Failure("request invalid")));

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            var result = await new TestEventSyncOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("request invalid", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async Task PostProcessorAbstractCallsOnBothAndOnSuccessWhenResultOk()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Protected().Setup("OnBoth", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult<TestResponse>>());
            processorMock.Protected().Setup("OnFailure", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult>());
            processorMock.Protected().Setup("OnSuccess", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<TestResponse>());

            await new TestEventOk()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Protected().Verify("OnBoth", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult<TestResponse>>());
            processorMock.Protected().Verify("OnFailure", Times.Never(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult>());
            processorMock.Protected().Verify("OnSuccess", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<TestResponse>());
        }

        [Fact]
        public async Task PostProcessorAbstractCallsOnFailureWhenResultFail()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Protected().Setup("OnBoth", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult<TestResponse>>());
            processorMock.Protected().Setup("OnFailure", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult>());
            processorMock.Protected().Setup("OnSuccess", ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<TestResponse>());


            await new TestEventFail()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Protected().Verify("OnBoth", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult<TestResponse>>());
            processorMock.Protected().Verify("OnFailure", Times.Once(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<IResult>());
            processorMock.Protected().Verify("OnSuccess", Times.Never(), ItExpr.IsAny<TestRequest>(), ItExpr.IsAny<TestResponse>());
        }

        [Fact]
        public async Task PostProcessorRunsWhenEventFails()
        {
            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            var result = await new TestEventFail()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("event failed", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        internal class TestEventFail : EventHandlerAbstract<TestRequest, TestResponse>
        {
            public override async Task<IResult<TestResponse>> Handle(TestRequest validRequest, CancellationToken cancellationToken = default(CancellationToken))
            {
                await Task.Delay(1);
                return Result.Fail<TestResponse>(new Failure("event failed"));
            }
        }

        internal class TestEventOk : EventHandlerAbstract<TestRequest, TestResponse>
        {
            public override async Task<IResult<TestResponse>> Handle(TestRequest validRequest, CancellationToken cancellationToken = default(CancellationToken))
            {
                await Task.Delay(1);
                return Result.Ok(new TestResponse { Name = "success" });
            }
        }

        internal class TestEventSyncFail : EventHandlerSyncAbstract<TestRequest, TestResponse>
        {
            protected override IResult<TestResponse> HandleSync(TestRequest request)
            {
                return Result.Fail<TestResponse>(new Failure("event failed"));
            }
        }

        internal class TestEventSyncOk : EventHandlerSyncAbstract<TestRequest, TestResponse>
        {
            protected override IResult<TestResponse> HandleSync(TestRequest request)
            {
                return Result.Ok(new TestResponse { Name = "success" });
            }
        }

        public class TestRequest { }

        public class TestResponse
        {
            public string Name { get; set; }
        }
    }
}
