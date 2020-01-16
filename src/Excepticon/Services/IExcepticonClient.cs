using System;
using System.Threading.Tasks;
using Excepticon.Model;

namespace Excepticon.Services
{
    public interface IExcepticonClient
    {
        void CaptureException(Exception ex);

        void CaptureException(ExceptionInstance instance);

        Task FlushAsync(TimeSpan timeout);
    }
}
