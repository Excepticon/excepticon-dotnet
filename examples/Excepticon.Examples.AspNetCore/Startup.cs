using Excepticon.AspNetCore;
using Excepticon.AspNetCore.Logging;
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

            // Add Excepticon to the specified IServiceCollection

            // Option 1 - Provide apiKey directly
            services.AddExcepticon("F1LU529+F8JSJYXnQMvBd3OGuncqzhlRVT0zLK7/FC8=");

            // Option 2 - Provide IConfiguration
            //            Looks for a section named "Excepticon" in the registered configuration providers.
            //            In this example, the section is included in the local.settings.json, but it could 
            //            be provided from any other configuration provider, such as another config file,
            //            environment variable, Azure app setting, etc...
            //services.AddExcepticon(_configuration);

            // Option 3 - Provide IConfiguration and custom section name
            //services.AddExcepticon(_configuration, "MyExcepticonSettings");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Adds Excepticon to the request execution pipeline
            app.UseExcepticon();

            app.UseMvc();
            
            // Run and navigate to /api/errors to have an error logged to Excepticon.
            // (Don't forget to set your ApiKey!)
        }
    }
}
