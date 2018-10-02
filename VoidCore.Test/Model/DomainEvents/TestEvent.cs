using VoidCore.Model.DomainEvents;

namespace VoidCore.Test.Model.DomainEvents
{
    public class TestEventOk : EventHandlerAbstract<TestRequest, TestResponse>
    {
        protected override Result<TestResponse> HandleInternal(TestRequest validRequest)
        {
            return Result.Ok(new TestResponse { Name = "success" });
        }
    }

    public class TestEventFail : EventHandlerAbstract<TestRequest, TestResponse>
    {
        protected override Result<TestResponse> HandleInternal(TestRequest validRequest)
        {
            return Result.Fail<TestResponse>("event failed");
        }
    }

    public class TestRequest { }

    public class TestResponse
    {
        public string Name { get; set; }
    }
}
