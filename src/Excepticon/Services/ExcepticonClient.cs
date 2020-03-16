using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Excepticon.Integrations;
using Excepticon.Model;
using Excepticon.Options;
using Microsoft.AspNetCore.Http;

namespace Excepticon.Services
{
    public class ExcepticonClient : IExcepticonClient, IDisposable
    {
        private volatile bool _disposed;
        private readonly ExcepticonOptions _options;
        private readonly List<ISdkIntegration> _integrations;

        internal IQueueManager QueueManager { get; }

        public ExcepticonClient(ExcepticonOptions options, IQueueManager queueManager)
        {
            QueueManager = queueManager ?? throw new ArgumentNullException(nameof(queueManager));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _integrations = options.Integrations;
            if (_integrations?.Any() ?? false)
                foreach (var integration in _integrations)
                    integration.Register(this, options);
        }

        public void CaptureException(Exception ex) => CaptureException(new ExceptionInstance(ex));

        public void CaptureException(Exception ex, HttpContext httpContext) => CaptureException(new ExceptionInstance(ex, httpContext));

        public void CaptureException(ExceptionInstance instance)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ExcepticonClient));

            DoSendExceptionInstance(instance);
        }

        public Task FlushAsync(TimeSpan timeout) => QueueManager.FlushQueueAsync(timeout);

        private void DoSendExceptionInstance(ExceptionInstance instance) => QueueManager.EnqueueExceptionInstance(instance);

        public void Dispose()
        {
            if (_disposed)
                return;
            
            _disposed = true;

            if (_integrations?.Count > 0)
                foreach (var integration in _integrations)
                    integration.Unregister(this);

            (QueueManager as IDisposable)?.Dispose();
        }
    }
}
