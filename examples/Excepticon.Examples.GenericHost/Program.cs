using System.IO;
using System.Threading.Tasks;
using Excepticon.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Excepticon.Examples.GenericHost
{
    class Program
    {
        static Task Main(string[] args) =>
            new HostBuilder()
                .ConfigureHostConfiguration(c =>
                {
                    c.SetBasePath(Directory.GetCurrentDirectory());
                    c.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((c, s) => s.AddHostedService<MyHostedService>())
                .ConfigureLogging((c, l) =>
                {
                    l.AddConfiguration(c.Configuration);
                    l.AddConsole();
                    l.AddExcepticon("{Your ApiKey Here");
                })
                .UseConsoleLifetime()
                .Build()
                .RunAsync();
    }
}
