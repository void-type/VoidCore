using System.Threading.Tasks;
using VoidCore.Model.DomainEvents;

namespace VoidCore.Test.Model.DomainEvents
{
    public class TestEventOk : DomainEventAbstract<TestRequest, TestResponse>
    {
        protected override async Task<Result<TestResponse>> HandleInternal(TestRequest validRequest)
        {
            return await Task.FromResult(
                Result.Ok(
                    new TestResponse { Name = "success" }));
        }
    }

    public class TestEventFail : DomainEventAbstract<TestRequest, TestResponse>
    {
        protected override async Task<Result<TestResponse>> HandleInternal(TestRequest validRequest)
        {
            return await Task.FromResult(
                Result.Fail<TestResponse>(
                    "event failed"));
        }
    }

    public class TestRequest { }

    public class TestResponse
    {
        public string Name { get; set; }
    }
}
