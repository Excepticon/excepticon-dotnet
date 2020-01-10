using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Excepticon.AspNetCore
{
    internal class ExcepticonStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            => e =>
            {
                e.UseExcepticon();

                next(e);
            };
    }
}
