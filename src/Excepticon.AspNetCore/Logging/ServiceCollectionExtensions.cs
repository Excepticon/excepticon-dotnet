using System;
using Excepticon.AspNetCore.Options;
using Excepticon.AspNetCore.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Excepticon.AspNetCore.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExcepticon(this IServiceCollection services)
        {
            services.TryAddSingleton(
                c => c.GetRequiredService<IOptions<ExcepticonOptions>>().Value);

            services.TryAddSingleton<IExcepticonExceptionInstancesService, ExcepticonExceptionInstancesService>();
            services.TryAddSingleton<IExcepticonClient, ExcepticonClient>();
            services.TryAddSingleton<IBackgroundWorker, BackgroundWorker>();

            services.TryAddSingleton<Func<IExcepticonClient>>(c =>
            {
                var excepticonClient = c.GetRequiredService<IExcepticonClient>();
                return () => excepticonClient;
            });
            
            return services;
        }
    }
}
