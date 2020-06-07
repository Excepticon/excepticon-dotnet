using System;
using System.Collections.Generic;
using System.Linq;
using Excepticon.Integrations;

namespace Excepticon.Options
{
    public class ExcepticonOptions
    {
        public ExcepticonOptions()
        {
            Integrations = new List<ISdkIntegration>
            {
                new UnhandledExceptionIntegration(),
                new ProcessExitIntegration()
            };
        }

        public ExcepticonOptions(string apiKey) : this()
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; set; }

        public bool ShouldSwallowExceptions { get; set; } = false;

        public TimeSpan ShutdownTimeout { get; } = new TimeSpan(0, 0, 0, 60);

        public int MaxQueueItems { get; } = 10;

        public bool FlushOnCompletedRequest { get; } = true;

        public TimeSpan FlushTimeout { get; } = new TimeSpan(0, 0, 0, 60);

        public string Url { get; set; } = "https://api.excepticon.io/";

        public string ExcludedExceptionTypes { get; set; }

        internal List<ISdkIntegration> Integrations { get; set; }

        public List<string> ExcludedExceptionTypeList => ExcludedExceptionTypes?.Split(';').ToList() ?? new List<string>();
    }
}
