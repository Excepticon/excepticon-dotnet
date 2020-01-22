using System;
using System.Threading;
using System.Threading.Tasks;
using Excepticon.Services;
using Microsoft.Extensions.Hosting;

namespace Excepticon.Examples.GenericHost
{
    internal class MyHostedService : IHostedService
    {
        private readonly IExcepticonClient _excepticonClient;

        public MyHostedService(IExcepticonClient excepticonClient)
        {
            _excepticonClient = excepticonClient ?? throw new ArgumentNullException(nameof(excepticonClient));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    throw new ApplicationException("This error will be sent to Excepticon.");
                }
                catch (Exception ex)
                {
                    _excepticonClient.CaptureException(ex);
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
