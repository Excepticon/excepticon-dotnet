using System;
using Excepticon.Internal;
using Excepticon.Options;
using Excepticon.Services;

namespace Excepticon.Integrations
{
    internal class ProcessExitIntegration : ISdkIntegration
    {
        private readonly IAppDomain _appDomain;
        private IExcepticonClient _client;

        public ProcessExitIntegration(IAppDomain appDomain = null)
        {
            _appDomain = appDomain ?? AppDomainAdapter.Instance;
        }

        public void Register(IExcepticonClient client, ExcepticonOptions options)
        {
            _client = client;
            _appDomain.ProcessExit += OnProcessExit;
        }

        public void Unregister(IExcepticonClient client)
        {
            _appDomain.ProcessExit -= OnProcessExit;
            _client = null;
        }

        internal void OnProcessExit(object sender, EventArgs e)
        {
            (_client as IDisposable)?.Dispose();
        }
    }
}
