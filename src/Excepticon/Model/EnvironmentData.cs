using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Excepticon.Model
{
    public class EnvironmentData
    {
        public EnvironmentData(HttpContext context)
        {
            AspNetCoreFeatures = context.Features.Select(f => f.Key.FullName).ToList();
        }

        public IList<string> AspNetCoreFeatures { get; }
    }
}
