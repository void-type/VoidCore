using Moq;
using System.Linq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Validation;
using Xunit;

namespace VoidCore.Test.Model.DomainEvents
{
    public class DomainEventTests
    {
        [Fact]
        public void EventHandledNoValidatorOrPostProcessor()
        {
            var domainEvent = new TestEventOk();

            var result = domainEvent.Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
        }

        [Fact]
        public void EventHandledNoValidatorWithPostProcessor()
        {
            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var domainEvent = new TestEventOk();
            domainEvent.AddPostProcessor(processorMock.Object);

            var result = domainEvent.Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }

        [Fact]
        public void EventHandledNoPostProcessorWhenRequestValid()
        {
            var validatorMock = new Mock<IValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Ok());

            var domainEvent = new TestEventOk();
            domainEvent.AddRequestValidator(validatorMock.Object);

            var result = domainEvent.Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
        }

        [Fact]
        public void EventHandledAndPostProcessorsRunWhenRequestValid()
        {
            var validatorMock = new Mock<IValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Ok());

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var domainEvent = new TestEventOk();
            domainEvent.AddRequestValidator(validatorMock.Object);
            domainEvent.AddPostProcessor(processorMock.Object);
            domainEvent.AddPostProcessor(processorMock.Object);

            var result = domainEvent.Handle(new TestRequest());

            Assert.Equal("success", result.Value.Name);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Exactly(2));
        }

        [Fact]
        public void EventNotHandledAndPostProcessorRunsWhenRequestInvalid()
        {
            var validatorMock = new Mock<IValidator<TestRequest>>();
            validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>())).Returns(Result.Fail<TestResponse>("request invalid"));

            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var domainEvent = new TestEventOk();
            domainEvent.AddRequestValidator(validatorMock.Object);
            domainEvent.AddPostProcessor(processorMock.Object);

            var result = domainEvent.Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("request invalid", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }

        [Fact]
        public void PostProcessorRunsWhenEventFails()
        {
            var processorMock = new Mock<IPostProcessor<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var domainEvent = new TestEventFail();
            domainEvent.AddPostProcessor(processorMock.Object);

            var result = domainEvent.Handle(new TestRequest());

            Assert.True(result.IsFailed);
            Assert.Equal("event failed", result.Failures.Single().Message);
            processorMock.Verify(p => p.Process(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }

        [Fact]
        public void PostProcessorAbstractCallsOnBothAndOnSuccessWhenResultOk()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));
            processorMock.Setup(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));
            processorMock.Setup(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var domainEvent = new TestEventOk();
            domainEvent.AddPostProcessor(processorMock.Object);

            domainEvent.Handle(new TestRequest());

            processorMock.Verify(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Never());
            processorMock.Verify(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
        }

        [Fact]
        public void PostProcessorAbstractCallsOnFailureWhenResultFail()
        {
            var processorMock = new Mock<PostProcessorAbstract<TestRequest, TestResponse>>();
            processorMock.Setup(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));
            processorMock.Setup(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));
            processorMock.Setup(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()));

            var domainEvent = new TestEventFail();
            domainEvent.AddPostProcessor(processorMock.Object);

            domainEvent.Handle(new TestRequest());

            processorMock.Verify(p => p.OnBoth(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnFailure(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Once());
            processorMock.Verify(p => p.OnSuccess(It.IsAny<TestRequest>(), It.IsAny<Result<TestResponse>>()), Times.Never());
        }
    }
}
