using System.Threading.Tasks;

namespace VoidCore.Model.Railway
{
    public interface IPostProcessor<in TRequest, TResponse>
    {
        Task Process(TRequest request, Result<TResponse> result);
    }
}
