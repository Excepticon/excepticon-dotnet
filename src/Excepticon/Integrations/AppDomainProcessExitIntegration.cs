using System;
using System.Diagnostics;
using Excepticon.Internal;
using Excepticon.Options;
using Excepticon.Services;

namespace Excepticon.Integrations
{
    internal class AppDomainProcessExitIntegration : ISdkIntegration
    {
        private readonly IAppDomain _appDomain;
        private IExcepticonClient _client;

        public AppDomainProcessExitIntegration(IAppDomain appDomain = null)
        {
            _appDomain = appDomain ?? AppDomainAdapter.Instance;
        }

        public void Register(IExcepticonClient client, ExcepticonOptions options)
        {
            Debug.Assert(client != null);
            _client = client;
            _appDomain.ProcessExit += HandleProcessExit;
        }

        public void Unregister(IExcepticonClient client)
        {
            _appDomain.ProcessExit -= HandleProcessExit;
            _client = null;
        }

        internal void HandleProcessExit(object sender, EventArgs e)
        {
            (_client as IDisposable)?.Dispose();
        }
    }
}
