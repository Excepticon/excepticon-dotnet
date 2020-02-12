using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Excepticon.Model;

namespace Excepticon.Services
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBackgroundWorker
    {
        bool EnqueueExceptionInstance(ExceptionInstance instance);

        Task FlushAsync(TimeSpan timeout);

        int QueuedItems { get; }
    }
}
