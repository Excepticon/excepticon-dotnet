using System;
using Excepticon.AspNetCore.Model;
using Excepticon.AspNetCore.Options;
using Excepticon.AspNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Excepticon.AspNetCore.Logging
{
    internal sealed class ExcepticonLogger : ILogger
    {
        private readonly ExcepticonOptions _excepticonOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExcepticonClient _excepticonClient;

        internal string CategoryName { get; }

        internal ExcepticonLogger(
            string categoryName,
            ExcepticonOptions excepticonOptions,
            IHttpContextAccessor httpContextAccessor,
            IExcepticonClient excepticonClient)
        {
            CategoryName = categoryName;
            _excepticonOptions = excepticonOptions ?? throw new ArgumentNullException(nameof(excepticonOptions));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _excepticonClient = excepticonClient ?? throw new ArgumentNullException(nameof(excepticonClient));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel != LogLevel.None;

        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (exception != null)
            {
                var exceptionInstance = new ExceptionInstance(exception, _httpContextAccessor.HttpContext);
                _excepticonClient.CaptureException(exceptionInstance);
            }
        }
    }
}
