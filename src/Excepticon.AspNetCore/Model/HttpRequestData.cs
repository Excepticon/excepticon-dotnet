using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Excepticon.AspNetCore.Model
{
    public class HttpRequestData
    {
        public HttpRequestData(HttpContext context)
        {
            var request = context.Request;

            Cookies = request.Cookies.Select(c => new { c.Key, c.Value }).ToDictionary(x => x.Key, x => x.Value);
            Headers = request.Headers.Select(h => new { h.Key, h.Value }).ToDictionary(x => x.Key, x => x.Value.ToString());
            Host = request.Host.Value;
            Method = request.Method;
            Path = request.Path;
            Protocol = request.Protocol;
            Query = request.Query.Select(q => new { q.Key, q.Value }).ToDictionary(x => x.Key, x => x.Value.ToString());
            QueryString = request.QueryString.HasValue? request.QueryString.Value : string.Empty;
            Scheme = request.Scheme;

            Session = new Dictionary<string, string>();
            if (context.Features.Get<ISessionFeature>() != null)
            {
                foreach (var k in context.Session.Keys)
                {
                    Session.Add(k, context.Session.GetString(k));
                }
            }
        }

        public IDictionary<string, string> Cookies { get; }

        public IDictionary<string, string> Headers { get; }

        public string Host { get; }

        public string Method { get; }

        public string Path { get; }

        public string Protocol { get; }

        public IDictionary<string, string> Query { get; }

        public IDictionary<string, string> Session { get; }

        public string QueryString { get; }

        public string Scheme { get; }
    }
}
