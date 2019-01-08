using Moq;
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
        public async void EventHandledAndPostProcessorsRunWhenRequestValid()
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
        public async void EventHandledNoPostProcessorWhenRequestValid()
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
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            var result = await new TestEventOk()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async void EventNotHandledAndPostProcessorRunsWhenRequestInvalid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                .Returns(Result.Fail<TestResponse>("request invalid"));

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
        public async void EventSyncHandledAndPostProcessorsRunWhenRequestValid()
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
        public async void EventSyncNotHandledAndPostProcessorRunsWhenRequestInvalid()
        {
            var validatorMock = new Mock<IRequestValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                .Returns(Result.Fail<TestResponse>("request invalid"));

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
        public async void PostProcessorAbstractCallsOnBothAndOnSuccessWhenResultOk()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));
            processorMock.Setup(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));
            processorMock.Setup(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            await new TestEventOk()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Verify(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Never());
            processorMock.Verify(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
        }

        [Fact]
        public async void PostProcessorAbstractCallsOnFailureWhenResultFail()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));
            processorMock.Setup(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));
            processorMock.Setup(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()));

            await new TestEventFail()
                .AddPostProcessor(processorMock.Object)
                .Handle(new TestRequest());

            processorMock.Verify(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<IResult<TestResponse>>()), Times.Never());
        }

        [Fact]
        public async void PostProcessorRunsWhenEventFails()
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
                await Task.Run(() => Thread.Sleep(10));
                return Result.Fail<TestResponse>("event failed");
            }
        }

        internal class TestEventOk : EventHandlerAbstract<TestRequest, TestResponse>
        {
            public override async Task<IResult<TestResponse>> Handle(TestRequest validRequest, CancellationToken cancellationToken = default(CancellationToken))
            {
                await Task.Run(() => Thread.Sleep(10));
                return Result.Ok(new TestResponse { Name = "success" });
            }
        }

        internal class TestEventSyncFail : EventHandlerSyncAbstract<TestRequest, TestResponse>
        {
            protected override IResult<TestResponse> HandleSync(TestRequest request)
            {
                return Result.Fail<TestResponse>("event failed");
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
