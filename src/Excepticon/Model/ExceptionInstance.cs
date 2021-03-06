﻿using Microsoft.AspNetCore.Http;
using System;

namespace Excepticon.Model
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

            if (Environment is null)
            {
                Environment = new EnvironmentData();
            }
        }

        public ExceptionInstance(Exception exception, HttpContext httpContext) : this(exception)
        {
            if (httpContext != null)
            {
                HttpRequest = new HttpRequestData(httpContext);
            }
            Environment = new EnvironmentData(httpContext);
        }

        public string Type { get; set; }

        public string FullyQualifiedType { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string Source { get; set; }

        public DateTimeOffset TimeOccurred { get; set; }

        public EnvironmentData Environment { get; set; }

        public TargetSite TargetSite { get; set; }

        public HttpRequestData HttpRequest { get; set; }
    }
}
