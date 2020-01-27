using Excepticon.AspNetCore;
using Excepticon.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Excepticon.Examples.AspNetCore
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", true, true);

            _configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);

            // Add Excepticon to the IServiceCollection
            services.AddExcepticon(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add Excepticon to the request execution pipeline
            app.UseExcepticon();

            app.UseMvc();
        }
    }
}
