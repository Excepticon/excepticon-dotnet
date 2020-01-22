using System;
using Excepticon.Options;
using Excepticon.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Excepticon.Logging
{
    [ProviderAlias("Excepticon")]
    public class ExcepticonLoggerProvider : ILoggerProvider
    {
        private readonly ExcepticonOptions _excepticonOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExcepticonClient _excepticonClient;

        public ExcepticonLoggerProvider(
            ExcepticonOptions excepticonOptions,
            IHttpContextAccessor httpContextAccessor,
            IExcepticonClient excepticonClient)
        {
            _excepticonOptions = excepticonOptions ?? throw new ArgumentNullException(nameof(excepticonOptions));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _excepticonClient = excepticonClient ?? throw new ArgumentNullException(nameof(excepticonClient));
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName) => new ExcepticonLogger(
            categoryName, 
            _excepticonOptions, 
            _httpContextAccessor,
            _excepticonClient);
    }
}
