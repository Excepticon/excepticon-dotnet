using System;
using Excepticon.AspNetCore.Options;
using Excepticon.AspNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Excepticon.AspNetCore.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddExcepticon(
            this ILoggingBuilder builder) =>
            builder.AddExcepticon((Action<ExcepticonOptions>) null);

        public static ILoggingBuilder AddExcepticon(
            this ILoggingBuilder builder, 
            string excepticonApiKey) =>
            builder.AddExcepticon(o => o.ApiKey = excepticonApiKey);

        public static ILoggingBuilder AddExcepticon(
            this ILoggingBuilder builder,
            Action<ExcepticonOptions> optionsConfiguration)
        {
            var options = new ExcepticonOptions();
            optionsConfiguration?.Invoke(options);

            builder.AddConfiguration();

            if (optionsConfiguration != null)
            {
                builder.Services.Configure(optionsConfiguration);
            }

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IConfigureOptions<ExcepticonOptions>, ExcepticonLoggingOptionsSetup>();
            builder.Services.AddSingleton<ILoggerProvider, ExcepticonLoggerProvider>();
            builder.Services.AddSingleton<IExcepticonClient, ExcepticonClient>();
            builder.Services.AddSingleton<IBackgroundWorker, BackgroundWorker>();

            builder.Services.AddExcepticon();
            return builder;
        }
    }
}
