using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using Xunit;

namespace VoidCore.Test.Model;

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
            .AddRequestValidator(validatorMock)
            .Handle(new TestRequest());

        Assert.True(result.IsSuccess);
        Assert.Equal("success", result.Value.Name);
    }

    [Fact]
    public async Task Event_is_handled_and_post_processors_run_with_no_validator()
    {
        var processorMock = MockProcessorWidget();

        var result = await new TestEventOk()
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsSuccess);
        Assert.Equal("success", result.Value.Name);
        processorMock.DidNotReceive().OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(1).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.Received(1).OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
    }

    [Fact]
    public async Task Event_is_not_handled_and_post_processors_run_when_request_is_invalid()
    {
        var validatorMock = MockIRequestValidatorFail();
        var processorMock = MockProcessorWidget();

        var result = await new TestEventOk()
            .AddRequestValidator(validatorMock)
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsFailed);
        Assert.Equal("request invalid", result.Failures.Single().Message);
        processorMock.Received(1).OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(1).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.DidNotReceive().OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
    }

    [Fact]
    public async Task Event_is_handled_and_post_processors_run_when_request_is_valid()
    {
        var validatorMock = MockIRequestValidatorOk();
        var processorMock = MockProcessorWidget();

        var result = await new TestEventOk()
            .AddRequestValidator(validatorMock)
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsSuccess);
        Assert.Equal("success", result.Value.Name);
        processorMock.DidNotReceive().OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(2).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.Received(2).OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
    }

    [Fact]
    public async Task Post_processor_runs_when_event_fails()
    {
        var processorMock = MockProcessorWidget();

        var result = await new TestEventFail()
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsFailed);
        Assert.Equal("event failed", result.Failures.Single().Message);
        processorMock.Received(1).OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(1).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.DidNotReceive().OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
    }

    [Fact]
    public async Task Post_processor_abstract_calls_OnBoth_and_OnSuccess_when_result_ok()
    {
        var validatorMock = MockIRequestValidatorOk();
        var processorMock = MockProcessorWidget();

        var result = await new TestEventOk()
            .AddRequestValidator(validatorMock)
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsSuccess);
        Assert.Equal("success", result.Value.Name);
        processorMock.DidNotReceive().OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(1).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.Received(1).OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
    }

    [Fact]
    public async Task Post_processor_abstract_calls_OnBoth_and_OnFailure_when_result_fail()
    {
        var processorMock = MockProcessorWidget();

        var result = await new TestEventFail()
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsFailed);
        Assert.Equal("event failed", result.Failures.Single().Message);
        processorMock.Received(1).OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(1).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.DidNotReceive().OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
    }

    [Fact]
    public async Task Post_processor_abstract_calls_OnBoth_and_OnFailure_when_request_invalid()
    {
        var validatorMock = MockIRequestValidatorFail();
        var processorMock = MockProcessorWidget();

        var result = await new TestEventOk()
            .AddRequestValidator(validatorMock)
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsFailed);
        Assert.Equal("request invalid", result.Failures.Single().Message);
        processorMock.Received(1).OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(1).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.DidNotReceive().OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
    }

    [Fact]
    public async Task Synchronous_event_is_handled_and_post_processors_run_when_request_is_valid()
    {
        var validatorMock = MockIRequestValidatorOk();
        var processorMock = MockProcessorWidget();

        var result = await new TestEventSyncOk()
            .AddRequestValidator(validatorMock)
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsSuccess);
        Assert.Equal("success", result.Value.Name);
        processorMock.DidNotReceive().OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(1).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.Received(1).OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
    }

    [Fact]
    public async Task Synchronous_event_is_not_handled_and_post_processors_run_when_request_is_invalid()
    {
        var validatorMock = MockIRequestValidatorFail();
        var processorMock = MockProcessorWidget();

        var result = await new TestEventSyncOk()
            .AddRequestValidator(validatorMock)
            .AddPostProcessor(new WidgetProcessor(processorMock))
            .Handle(new TestRequest());

        Assert.True(result.IsFailed);
        Assert.Equal("request invalid", result.Failures.Single().Message);
        processorMock.Received(1).OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.Received(1).OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.DidNotReceive().OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
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

    private static IRequestValidator<TestRequest> MockIRequestValidatorOk()
    {
        var validatorMock = Substitute.For<IRequestValidator<TestRequest>>();
        validatorMock.Validate(Arg.Any<TestRequest>()).Returns(x => Result.Ok(x[0] as TestRequest));
        return validatorMock;
    }

    private static IRequestValidator<TestRequest> MockIRequestValidatorFail()
    {
        var validatorMock = Substitute.For<IRequestValidator<TestRequest>>();
        validatorMock.Validate(Arg.Any<TestRequest>())
            .Returns(Result.Fail<TestRequest>(new Failure("request invalid")));
        return validatorMock;
    }

    private static IProcessorWidget MockProcessorWidget()
    {
        var processorMock = Substitute.For<IProcessorWidget>();
        processorMock.OnFailure(Arg.Any<TestRequest>(), Arg.Any<IEnumerable<IFailure>>());
        processorMock.OnBoth(Arg.Any<TestRequest>(), Arg.Any<IResult<TestResponse>>());
        processorMock.OnSuccess(Arg.Any<TestRequest>(), Arg.Any<TestResponse>());
        return processorMock;
    }

    // Used to verify protected methods
    public interface IProcessorWidget
    {
        void OnFailure(TestRequest request, IEnumerable<IFailure> failures);
        void OnBoth(TestRequest request, IResult<TestResponse> result);
        void OnSuccess(TestRequest request, TestResponse response);
    }

    public class WidgetProcessor : PostProcessorAbstract<TestRequest, TestResponse>
    {
        private readonly IProcessorWidget _widget;

        public WidgetProcessor(IProcessorWidget widget)
        {
            _widget = widget;
        }

        protected override void OnFailure(TestRequest request, IEnumerable<IFailure> failures)
        {
            _widget.OnFailure(request, failures);
        }

        protected override void OnBoth(TestRequest request, IResult<TestResponse> result)
        {
            _widget.OnBoth(request, result);
        }

        protected override void OnSuccess(TestRequest request, TestResponse response)
        {
            _widget.OnSuccess(request, response);
        }
    }
}
