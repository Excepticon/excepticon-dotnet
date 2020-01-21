using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Excepticon.Model
{
    public class EnvironmentData
    {
        public EnvironmentData(HttpContext context) : this()
        {
            AspNetCoreFeatures = context.Features.Select(f => f.Key.FullName).ToList();
        }

        public EnvironmentData()
        {
            CurrentDirectory = Environment.CurrentDirectory;

            foreach (DictionaryEntry e in Environment.GetEnvironmentVariables())
            {
                EnvironmentVariables.Add(e.Key.ToString(), e.Value.ToString());
            }
        }

        public IList<string> AspNetCoreFeatures { get; }

        public IDictionary<string, string> EnvironmentVariables { get; } = new Dictionary<string, string>();

        public string CurrentDirectory { get; }
    }
}
