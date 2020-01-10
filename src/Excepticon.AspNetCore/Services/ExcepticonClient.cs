using System;
using System.Threading.Tasks;
using Excepticon.AspNetCore.Model;
using Excepticon.AspNetCore.Options;

namespace Excepticon.AspNetCore.Services
{
    public class ExcepticonClient : IExcepticonClient, IDisposable
    {
        private volatile bool _disposed;
        private readonly ExcepticonOptions _options;

        internal IBackgroundWorker Worker { get; }

        public ExcepticonClient(ExcepticonOptions options, IBackgroundWorker worker)
        {
            Worker = worker ?? throw new ArgumentNullException(nameof(worker));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void CaptureException(ExceptionInstance instance)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ExcepticonClient));
            }

            DoSendExceptionInstance(instance);
        }

        public Task FlushAsync(TimeSpan timeout) => Worker.FlushAsync(timeout);

        private void DoSendExceptionInstance(ExceptionInstance instance)
        {
            Worker.EnqueueExceptionInstance(instance);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            (Worker as IDisposable)?.Dispose();
        }
    }
}
