using System.Threading.Tasks;

namespace VoidCore.Model.Railway
{
    public interface IDomainEventHandler<in TRequest, TResponse>
    {
        Task<Result<TResponse>> Handle(TRequest request);
    }
}