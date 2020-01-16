using System;
using System.Threading.Tasks;
using Excepticon.Model;

namespace Excepticon.Services
{
    public interface IBackgroundWorker
    {
        bool EnqueueExceptionInstance(ExceptionInstance instance);

        Task FlushAsync(TimeSpan timeout);

        int QueuedItems { get; }
    }
}
