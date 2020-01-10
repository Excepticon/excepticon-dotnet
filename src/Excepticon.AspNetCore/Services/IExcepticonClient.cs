using System;
using System.Threading.Tasks;
using Excepticon.AspNetCore.Model;

namespace Excepticon.AspNetCore.Services
{
    public interface IExcepticonClient
    {
        void CaptureException(ExceptionInstance instance);

        Task FlushAsync(TimeSpan timeout);
    }
}
