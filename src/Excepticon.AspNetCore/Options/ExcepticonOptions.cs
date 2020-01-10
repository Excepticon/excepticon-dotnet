using System;

namespace Excepticon.AspNetCore.Options
{
    public class ExcepticonOptions
    {
        public string ApiKey { get; set; }

        public bool ShouldSwallowExceptions { get; set; } = false;

        public TimeSpan ShutdownTimeout { get; } = new TimeSpan(0, 0, 0, 60);

        public int MaxQueueItems { get; } = 10;

        public bool FlushOnCompletedRequest { get; } = true;

        public TimeSpan FlushTimeout { get; } = new TimeSpan(0, 0, 0, 60);

        public string Url { get; set; } = "https://api.excepticon.io/";
    }
}
