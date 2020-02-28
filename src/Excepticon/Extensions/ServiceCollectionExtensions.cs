using System;
using Excepticon.Options;
using Excepticon.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Excepticon.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExcepticon(this IServiceCollection services)
        {
            services.TryAddSingleton(
                c => c.GetRequiredService<IOptions<ExcepticonOptions>>().Value);

            services.TryAddSingleton<IExcepticonExceptionInstancesService, ExcepticonExceptionInstancesService>();
            services.TryAddSingleton<IExcepticonClient, ExcepticonClient>();
            services.TryAddSingleton<IQueueManager, QueueManager>();

            services.TryAddSingleton<Func<IExcepticonClient>>(c =>
            {
                var excepticonClient = c.GetRequiredService<IExcepticonClient>();
                return () => excepticonClient;
            });
            
            return services;
        }

        public static IServiceCollection AddExcepticon(
            this IServiceCollection services, 
            IConfiguration config,
            string sectionName = "Excepticon")
        {
            var section = config.GetSection(sectionName);
            services.Configure<ExcepticonOptions>(section);

            return AddExcepticon(services);
        }

        public static IServiceCollection AddExcepticon(
            this IServiceCollection services,
            string apiKey)
        {
            var excepticonOptions = new OptionsWrapper<ExcepticonOptions>(new ExcepticonOptions { ApiKey = apiKey });
            
            services.TryAddSingleton<IOptions<ExcepticonOptions>>(excepticonOptions);
            
            return AddExcepticon(services);
        }
    }
}
