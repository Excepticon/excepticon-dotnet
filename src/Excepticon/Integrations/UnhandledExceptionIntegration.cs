﻿using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
using Excepticon.Internal;
using Excepticon.Options;
using Excepticon.Services;

namespace Excepticon.Integrations
{
    internal class UnhandledExceptionIntegration : ISdkIntegration
    {
        private readonly IAppDomain _appDomain;
        private IExcepticonClient _client;

        internal UnhandledExceptionIntegration(IAppDomain appDomain = null) => _appDomain = appDomain ?? AppDomainAdapter.Instance;

        public void Register(IExcepticonClient client, ExcepticonOptions options)
        {
            _client = client;
            _appDomain.UnhandledException += OnUnhandledException;
        }

        public void Unregister(IExcepticonClient client)
        {
            _appDomain.UnhandledException -= OnUnhandledException;
            _client = null;
        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        internal void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                _client?.CaptureException(ex);

            if (e.IsTerminating)
                (_client as IDisposable)?.Dispose();
        }
    }
}
