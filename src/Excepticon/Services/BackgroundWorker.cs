using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Excepticon.Model;
using Excepticon.Options;

namespace Excepticon.Services
{
    public class BackgroundWorker : IBackgroundWorker, IDisposable
    {
        private readonly ExcepticonOptions _options;
        private volatile bool _disposed;
        private readonly CancellationTokenSource _shutdownSource;
        private readonly ConcurrentQueue<ExceptionInstance> _queue;
        private readonly SemaphoreSlim _queuedEventSemaphore;
        private int _currentItems;

        private event EventHandler OnFlushObjectReceived;

        public BackgroundWorker(
            IExcepticonExceptionInstancesService excepticonExceptionInstancesService,
            ExcepticonOptions options,
            CancellationTokenSource shutdownSource = null,
            ConcurrentQueue<ExceptionInstance> queue = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _queuedEventSemaphore = new SemaphoreSlim(0, _options.MaxQueueItems);

            _shutdownSource = shutdownSource ?? new CancellationTokenSource();
            _queue = queue ?? new ConcurrentQueue<ExceptionInstance>();

            WorkerTask = Task.Run(
                async () => await WorkerAsync(
                        _queue,
                        _options,
                        excepticonExceptionInstancesService,
                        _queuedEventSemaphore,
                        _shutdownSource.Token)
                    .ConfigureAwait(false));
        }

        public int QueuedItems { get; }

        internal Task WorkerTask { get; }

        public bool EnqueueExceptionInstance(ExceptionInstance instance)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(BackgroundWorker));
            }

            if (instance == null)
            {
                return false;
            }

            if (Interlocked.Increment(ref _currentItems) > _options.MaxQueueItems)
            {
                Interlocked.Decrement(ref _currentItems);
                return false;
            }

            _queue.Enqueue(instance);
            _queuedEventSemaphore.Release();
            return true;
        }

        private async Task WorkerAsync(
            ConcurrentQueue<ExceptionInstance> queue,
            ExcepticonOptions options,
            IExcepticonExceptionInstancesService exceptionInstancesExcepticonExceptionInstancesService,
            SemaphoreSlim queuedEventSemaphore,
            CancellationToken cancellation)
        {
            var shutdownTimeout = new CancellationTokenSource();
            var shutdownRequested = false;

            try
            {
                while (!shutdownTimeout.IsCancellationRequested)
                {
                    // If the cancellation was signaled,
                    // set the latest we can keep reading off of the queue (while there's still stuff to read)
                    // No longer synchronized with queuedEventSemaphore (Enqueue will throw object disposed),
                    // run until the end of the queue or shutdownTimeout
                    if (!shutdownRequested)
                    {
                        try
                        {
                            await queuedEventSemaphore.WaitAsync(cancellation).ConfigureAwait(false);
                        }
                        // Cancellation requested, scheduled shutdown but continue in case there are more items
                        catch (OperationCanceledException)
                        {
                            if (options.ShutdownTimeout == TimeSpan.Zero)
                            {
                                return;
                            }

                            shutdownTimeout.CancelAfter(options.ShutdownTimeout);
                            shutdownRequested = true;
                        }
                    }

                    if (queue.TryPeek(out var exceptionInstance))
                    {
                        try
                        {
                            var task = exceptionInstancesExcepticonExceptionInstancesService.PostExceptionInstance(exceptionInstance, _options.ApiKey);
                            await task.ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            queue.TryDequeue(out _);
                            Interlocked.Decrement(ref _currentItems);
                            OnFlushObjectReceived?.Invoke(exceptionInstance, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        Debug.Assert(shutdownRequested);

                        // Empty queue. Exit.
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                queuedEventSemaphore.Dispose();
            }
        }

        public async Task FlushAsync(TimeSpan timeout)
        {
            if (_disposed)
            {
                return;
            }

            if (_queue.Count == 0)
            {
                return;
            }

            // Start timer from here.
            var timeoutSource = new CancellationTokenSource();
            timeoutSource.CancelAfter(timeout);
            var flushSuccessSource = new CancellationTokenSource();

            var timeoutWithShutdown = CancellationTokenSource.CreateLinkedTokenSource(
                timeoutSource.Token,
                _shutdownSource.Token,
                flushSuccessSource.Token);

            var counter = 0;
            var depth = int.MaxValue;

            void EventFlushedCallback(object objProcessed, EventArgs _)
            {
                // ReSharper disable once AccessToModifiedClosure
                if (Interlocked.Increment(ref counter) >= depth)
                {
                    try
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        flushSuccessSource.Cancel();
                    }
                    catch // Timeout or Shutdown might have been called so this token was disposed.
                    {
                    } // Flush will release when timeout is hit.
                }
            }

            OnFlushObjectReceived += EventFlushedCallback; // Started counting events
            try
            {
                var trackedDepth = _queue.Count;
                if (trackedDepth == 0) // now we're subscribed and counting, make sure it's not already empty.
                {
                    return;
                }

                Interlocked.Exchange(ref depth, trackedDepth);

                if (counter >= depth) // When the worker finished flushing before we set the depth
                {
                    return;
                }

                // Await until event is flushed or one of the tokens triggers
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
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            // Immediately requests the Worker to stop.
            _shutdownSource.Cancel();

            // If there's anything in the queue, it'll keep running until 'shutdownTimeout' is reached
            // If the queue is empty it will quit immediately
            WorkerTask.Wait(_options.ShutdownTimeout);
        }
    }
}