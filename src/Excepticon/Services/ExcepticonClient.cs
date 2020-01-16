using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Excepticon.Integrations;
using Excepticon.Model;
using Excepticon.Options;

namespace Excepticon.Services
{
    public class ExcepticonClient : IExcepticonClient, IDisposable
    {
        private volatile bool _disposed;
        private readonly ExcepticonOptions _options;
        private readonly List<ISdkIntegration> _integrations;

        internal IBackgroundWorker Worker { get; }

        public ExcepticonClient(ExcepticonOptions options, IBackgroundWorker worker)
        {
            Worker = worker ?? throw new ArgumentNullException(nameof(worker));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _integrations = options.Integrations;
            if (_integrations?.Count > 0)
            {
                foreach (var integration in _integrations)
                {
                    integration.Register(this, options);
                }
            }
        }

        public void CaptureException(Exception ex)
        {
            var instance = new ExceptionInstance(ex);

            CaptureException(instance);
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

            if (_integrations?.Count > 0)
            {
                foreach (var integration in _integrations)
                {
                    integration.Unregister(this);
                }
            }

            (Worker as IDisposable)?.Dispose();
        }
    }
}
