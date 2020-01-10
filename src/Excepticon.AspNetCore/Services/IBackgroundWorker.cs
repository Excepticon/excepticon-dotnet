using System;
using System.Threading.Tasks;
using Excepticon.AspNetCore.Model;

namespace Excepticon.AspNetCore.Services
{
    public interface IBackgroundWorker
    {
        bool EnqueueExceptionInstance(ExceptionInstance instance);

        Task FlushAsync(TimeSpan timeout);

        int QueuedItems { get; }
    }
}
