using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Excepticon.Model;

namespace Excepticon.Services
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IQueueManager
    {
        bool EnqueueExceptionInstance(ExceptionInstance instance);

        Task FlushQueueAsync(TimeSpan timeout);
    }
}
