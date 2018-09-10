using System;
using System.Threading.Tasks;
using VoidCore.Model.DomainEvents;

namespace VoidCore.Test.Model.DomainEvents
{
    public class TestEventOk : DomainEventAbstract<TestRequest, TestResponse>
    {
        protected async override Task<Result<TestResponse>> HandleInternal(TestRequest request)
        {
            return await Task.FromResult(
                Result.Ok(
                    new TestResponse { Name = "success" }));
        }
    }

    public class TestEventFail : DomainEventAbstract<TestRequest, TestResponse>
    {
        protected async override Task<Result<TestResponse>> HandleInternal(TestRequest request)
        {
            return await Task.FromResult(
                Result.Fail<TestResponse>(
                    "event failed"));
        }
    }

    public class TestRequest
    {
        public int Id { get; set; } = 1;
    }

    public class TestResponse
    {
        public string Name { get; set; }
    }
}
