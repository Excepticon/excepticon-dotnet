using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Excepticon.Model;
using Excepticon.Options;

namespace Excepticon.Services
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class QueueManager : IQueueManager, IDisposable
    {
        private readonly IExcepticonExceptionInstancesService _excepticonExceptionInstancesService;
        private readonly ExcepticonOptions _options;
        private volatile bool _disposed;
        private readonly CancellationTokenSource _shutdownSource;
        private readonly ConcurrentQueue<ExceptionInstance> _queue;
        private readonly SemaphoreSlim _queuedEventSemaphore;
        private int _currentItems;

        private event EventHandler OnFlushObjectReceived;

        public QueueManager(
            IExcepticonExceptionInstancesService excepticonExceptionInstancesService,
            ExcepticonOptions options,
            CancellationTokenSource shutdownSource = null,
            ConcurrentQueue<ExceptionInstance> queue = null)
        {
            _excepticonExceptionInstancesService = excepticonExceptionInstancesService ?? throw new ArgumentNullException(nameof(excepticonExceptionInstancesService));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _shutdownSource = shutdownSource ?? new CancellationTokenSource();
            _queue = queue ?? new ConcurrentQueue<ExceptionInstance>();

            _queuedEventSemaphore = new SemaphoreSlim(0, _options.MaxQueueItems);
            
            WorkerTask = Task.Run(async () => await WorkerAsync(_shutdownSource.Token).ConfigureAwait(false));
        }

        public bool EnqueueExceptionInstance(ExceptionInstance instance)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(QueueManager));

            if (instance == null) return false;

            if (Interlocked.Increment(ref _currentItems) <= _options.MaxQueueItems)
            {
                _queue.Enqueue(instance);
                _queuedEventSemaphore.Release();
                return true;
            }

            Interlocked.Decrement(ref _currentItems);
            return false;
        }

        private Task WorkerTask { get; }

        private async Task WorkerAsync(CancellationToken cancellation)
        {
            var shutdownTimeout = new CancellationTokenSource();
            var shutdownRequested = false;

            try
            {
                while (!shutdownTimeout.IsCancellationRequested)
                {
                    if (!shutdownRequested)
                    {
                        try
                        {
                            await _queuedEventSemaphore.WaitAsync(cancellation).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            if (_options.ShutdownTimeout == TimeSpan.Zero) return;

                            shutdownTimeout.CancelAfter(_options.ShutdownTimeout);
                            shutdownRequested = true;
                        }
                    }

                    if (_queue.TryPeek(out var exceptionInstance))
                    {
                        try
                        {
                            var task = _excepticonExceptionInstancesService.PostExceptionInstance(exceptionInstance, _options.ApiKey);
                            await task.ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                        finally
                        {
                            _queue.TryDequeue(out exceptionInstance);
                            Interlocked.Decrement(ref _currentItems);
                            OnFlushObjectReceived?.Invoke(exceptionInstance, EventArgs.Empty);
                        }
                    }
                    else return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _queuedEventSemaphore.Dispose();
            }
        }

        public async Task FlushQueueAsync(TimeSpan timeout)
        {
            if (_disposed || !_queue.Any()) return;

            var timeoutSource = new CancellationTokenSource();
            timeoutSource.CancelAfter(timeout);
            var flushSuccessSource = new CancellationTokenSource();

            var timeoutWithShutdown = CancellationTokenSource.CreateLinkedTokenSource(
                timeoutSource.Token,
                _shutdownSource.Token,
                flushSuccessSource.Token);

            var counter = 0;
            var depth = int.MaxValue;

            void EventFlushedCallback(object objProcessed, EventArgs eventArgs)
            {
                if (Interlocked.Increment(ref counter) >= depth)
                {
                    try
                    {
                        flushSuccessSource.Cancel();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            OnFlushObjectReceived += EventFlushedCallback;
            try
            {
                var trackedDepth = _queue.Count;
                if (trackedDepth == 0) return;

                Interlocked.Exchange(ref depth, trackedDepth);

                if (counter >= depth) return;

                await Task.Delay(timeout, timeoutWithShutdown.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                OnFlushObjectReceived -= EventFlushedCallback;
                timeoutWithShutdown.Dispose();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _shutdownSource.Cancel();
                WorkerTask.Wait(_options.ShutdownTimeout);
            }
        }
    }
}