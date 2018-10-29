using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Domain;
using Xunit;

namespace VoidCore.Test.Model.Domain
{
    public class EventTests
    {
        [Fact]
        public async void EventHandledAndPostProcessorsRunWhenRequestValid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Ok());

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Exactly(2));
        }

        [Fact]
        public async void EventHandledNoPostProcessorWhenRequestValid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Ok());

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
        }

        [Fact]
        public async void EventHandledNoValidatorOrPostProcessor()
        {
            var domainEvent = new TestEventOk();

            var result = await domainEvent.Handle(new TestRequest());

            Assert.True(result.IsSuccess);
            Assert.Equal("success", result.Value.Name);
        }

        [Fact]
        public async void EventHandledNoValidatorWithPostProcessor()
        {
            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var result = await new TestEventOk()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async void EventNotHandledAndPostProcessorRunsWhenRequestInvalid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Fail<TestResponse>("request invalid"));

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var result = await new TestEventOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("request invalid", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async void EventSyncHandledAndPostProcessorsRunWhenRequestValid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Ok());

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var result = await new TestEventSyncOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Exactly(1));
        }

        [Fact]
        public async void EventSyncNotHandledAndPostProcessorRunsWhenRequestInvalid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Fail<TestResponse>("request invalid"));

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var result = await new TestEventSyncOk()
                .AddRequestValidator(validatorMock.Object)
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("request invalid", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async void PostProcessorAbstractCallsOnBothAndOnSuccessWhenResultOk()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));
            processorMock.Setup(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));
            processorMock.Setup(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            await new TestEventOk()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Verify(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Never());
            processorMock.Verify(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async void PostProcessorAbstractCallsOnFailureWhenResultFail()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));
            processorMock.Setup(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));
            processorMock.Setup(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            await new TestEventFail()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Verify(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Never());
        }

        [Fact]
        public async void PostProcessorRunsWhenEventFails()
        {
            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var result = await new TestEventFail()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("event failed", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }
    }

    public class TestEventFail : EventHandlerAbstract<TestRequest, TestResponse>
    {
        public override async Task<Result<TestResponse>> Handle(TestRequest validRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() => Thread.Sleep(10));
            return Result.Fail<TestResponse>("event failed");
        }
    }

    public class TestEventOk : EventHandlerAbstract<TestRequest, TestResponse>
    {
        public override async Task<Result<TestResponse>> Handle(TestRequest validRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() => Thread.Sleep(10));
            return Result.Ok(new TestResponse { Name = "success" });
        }
    }

    public class TestEventSyncFail : EventHandlerSyncAbstract<TestRequest, TestResponse>
    {
        protected override Result<TestResponse> HandleSync(TestRequest request)
        {
            return Result.Fail<TestResponse>("event failed");
        }
    }

    public class TestEventSyncOk : EventHandlerSyncAbstract<TestRequest, TestResponse>
    {
        protected override Result<TestResponse> HandleSync(TestRequest request)
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
