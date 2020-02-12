using Excepticon.AspNetCore.Middleware;
using Excepticon.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Excepticon.AspNetCore
{
    public static class UseExcepticonExtensions
    {
        public static void UseExcepticon(
            this IApplicationBuilder appBuilder)
        {
            var options = appBuilder.ApplicationServices.GetService<IOptions<ExcepticonOptions>>();

            appBuilder.UseMiddleware(typeof(ExcepticonMiddleware), options);
        }
    }
}
