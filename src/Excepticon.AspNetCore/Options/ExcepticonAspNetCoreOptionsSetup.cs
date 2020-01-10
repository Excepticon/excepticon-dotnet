using Excepticon.AspNetCore.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Excepticon.AspNetCore.Options
{

    internal class ExcepticonAspNetCoreOptionsSetup : ConfigureFromConfigurationOptions<ExcepticonOptions>
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExcepticonAspNetCoreOptionsSetup(
            ILoggerProviderConfiguration<ExcepticonLoggerProvider> providerConfiguration,
            IHostingEnvironment hostingEnvironment)
            : base(providerConfiguration.Configuration)
            => _hostingEnvironment = hostingEnvironment;

        public override void Configure(ExcepticonOptions options)
        {
            base.Configure(options);
        }
    }
}
