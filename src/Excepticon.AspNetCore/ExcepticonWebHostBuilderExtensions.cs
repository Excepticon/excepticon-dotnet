﻿using System;
using Excepticon.AspNetCore.Options;
using Excepticon.Extensions;
using Excepticon.Logging;
using Excepticon.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Excepticon.AspNetCore
{
    public static class ExcepticonWebHostBuilderExtensions
    {
        public static IWebHostBuilder UseExcepticon(this IWebHostBuilder builder) =>
            UseExcepticon(builder, (Action<ExcepticonOptions>) null);
        
        public static IWebHostBuilder UseExcepticon(this IWebHostBuilder builder, string excepticonApiKey) =>
            builder.UseExcepticon(o => o.ApiKey = excepticonApiKey);

        public static IWebHostBuilder UseExcepticon(this IWebHostBuilder builder, Action<ExcepticonOptions> configureOptions) =>
            builder.UseExcepticon((context, options) => configureOptions?.Invoke(options));

        public static IWebHostBuilder UseExcepticon(this IWebHostBuilder builder, Action<WebHostBuilderContext, ExcepticonOptions> configureOptions)
        {
            builder.ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration();

                var section = context.Configuration.GetSection("Excepticon");
                logging.Services.Configure<ExcepticonOptions>(section);

                if (configureOptions != null)
                {
                    logging.Services.Configure<ExcepticonOptions>(options =>
                    {
                        configureOptions(context, options);
                    });
                }

                logging.Services.AddSingleton<IConfigureOptions<ExcepticonOptions>, ExcepticonAspNetCoreOptionsSetup>();
                logging.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                logging.Services.AddSingleton<ILoggerProvider, ExcepticonLoggerProvider>();

                logging.AddFilter<ExcepticonLoggerProvider>(
                    "Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware",
                    LogLevel.None);

                logging.Services.AddExcepticon();
            });

            return builder;
        }
    }
}
