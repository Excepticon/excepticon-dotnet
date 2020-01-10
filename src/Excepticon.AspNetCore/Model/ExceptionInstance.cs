using System;
using Microsoft.AspNetCore.Http;

namespace Excepticon.AspNetCore.Model
{
    public class ExceptionInstance
    {
        public ExceptionInstance(Exception exception)
        {
            Type = exception.GetType().Name;
            FullyQualifiedType = exception.GetType().FullName;
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            Source = exception.Source;
            TimeOccurred = DateTimeOffset.UtcNow;
            TargetSite = exception.TargetSite != null ? new TargetSite(exception.TargetSite) : null;
        }

        public ExceptionInstance(Exception exception, HttpContext httpContext) : this(exception)
        {
            HttpRequest = new HttpRequestData(httpContext);
            Environment = new EnvironmentData(httpContext);
        }

        public string Type { get; set; }

        public string FullyQualifiedType { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string Source { get; set; }

        public DateTimeOffset TimeOccurred { get; set; }

        public HttpRequestData HttpRequest { get; set; }

        public EnvironmentData Environment { get; set; }

        public TargetSite TargetSite { get; set; }
    }
}
