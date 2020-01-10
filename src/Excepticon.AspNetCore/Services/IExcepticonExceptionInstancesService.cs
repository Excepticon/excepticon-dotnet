using System.Threading.Tasks;
using Excepticon.AspNetCore.Model;

namespace Excepticon.AspNetCore.Services
{
    public interface IExcepticonExceptionInstancesService
    {
        Task PostExceptionInstance(ExceptionInstance exceptionInstance, string excepticonApiKey);
    }
}
