﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Excepticon.Options;
using Excepticon.Services;

namespace Excepticon
{
    public static class ExcepticonSdk
    {
        private static ExcepticonClient _client;

        public static IDisposable Init(string apiKey)
        {
            return Init(new ExcepticonOptions(apiKey));
        }

        public static IDisposable Init(ExcepticonOptions options)
        {
            var exceptionInstancesService = new ExcepticonExceptionInstancesService(options);
            var backgroundWorker = new BackgroundWorker(exceptionInstancesService, options);
            return UseClient(new ExcepticonClient(options, backgroundWorker));
        }

        public static void CaptureException(Exception ex) => _client.CaptureException(ex);

        public static Task FlushAsync(TimeSpan timeout) => _client.FlushAsync(timeout);

        internal static IDisposable UseClient(ExcepticonClient client)
        {
            var existingClient = Interlocked.Exchange(ref _client, client);
            existingClient?.Dispose();
            return client;
        }
    }
}
