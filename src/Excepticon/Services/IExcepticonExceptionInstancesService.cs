using System.ComponentModel;
using System.Threading.Tasks;
using Excepticon.Model;

namespace Excepticon.Services
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IExcepticonExceptionInstancesService
    {
        Task PostExceptionInstance(ExceptionInstance exceptionInstance, string excepticonApiKey);
    }
}
