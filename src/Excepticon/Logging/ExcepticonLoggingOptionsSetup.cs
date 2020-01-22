using Excepticon.Options;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Excepticon.Logging
{
    internal class ExcepticonLoggingOptionsSetup : ConfigureFromConfigurationOptions<ExcepticonOptions>
    {
        public ExcepticonLoggingOptionsSetup(
            ILoggerProviderConfiguration<ExcepticonLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        { }
    }
}
