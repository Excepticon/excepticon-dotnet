using System.Threading.Tasks;
using Excepticon.Model;

namespace Excepticon.Services
{
    public interface IExcepticonExceptionInstancesService
    {
        Task PostExceptionInstance(ExceptionInstance exceptionInstance, string excepticonApiKey);
    }
}
