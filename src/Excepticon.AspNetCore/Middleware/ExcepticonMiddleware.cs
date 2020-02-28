using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Excepticon.Model;
using Excepticon.Options;
using Excepticon.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Excepticon.AspNetCore.Middleware
{
    internal class ExcepticonMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Func<IExcepticonClient> _excepticonClientAccessor;
        private readonly ExcepticonOptions _options;
        private readonly ILogger<ExcepticonMiddleware> _logger;

        public ExcepticonMiddleware(
            RequestDelegate next,
            Func<IExcepticonClient> excepticonClientAccessor,
            IOptions<ExcepticonOptions> options,
            ILogger<ExcepticonMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _excepticonClientAccessor = excepticonClientAccessor ?? throw new ArgumentNullException(nameof(excepticonClientAccessor));
            _options = options?.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var excepticonClient = _excepticonClientAccessor();

            if (_options.FlushOnCompletedRequest)
            {
                context.Response.OnCompleted(async () =>
                {
                    await excepticonClient.FlushAsync(_options.FlushTimeout).ConfigureAwait(false);
                });
            }

            try
            {
                await _next(context).ConfigureAwait(false);

                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionFeature?.Error != null)
                    CaptureException(exceptionFeature.Error);
            }
            catch (Exception e)
            {
                CaptureException(e);

                ExceptionDispatchInfo.Capture(e).Throw();
            }

            void CaptureException(Exception e)
            {
                if (!string.IsNullOrWhiteSpace(_options.ApiKey))
                {
                    var exceptionInstance = new ExceptionInstance(e, context);

                    _logger?.LogTrace("Sending exception to Excepticon.", exceptionInstance);

                    excepticonClient.CaptureException(exceptionInstance);
                }
            }
        }
    }
}
