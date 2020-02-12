using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Excepticon.Constants;
using Excepticon.Model;
using Excepticon.Options;
using Newtonsoft.Json;

namespace Excepticon.Services
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ExcepticonExceptionInstancesService : IExcepticonExceptionInstancesService
    {
        private readonly HttpClient _httpClient;

        public ExcepticonExceptionInstancesService(ExcepticonOptions options)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.Url, UriKind.Absolute)
            };
        }

        public async Task PostExceptionInstance(ExceptionInstance exceptionInstance, string excepticonApiKey)
        {
            var content = new StringContent(JsonConvert.SerializeObject(exceptionInstance));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.Add(CustomHeaders.ExcepticonApiKey, new[] { excepticonApiKey });
            var result = await _httpClient.PostAsync("exceptions", content);
        }
    }
}
