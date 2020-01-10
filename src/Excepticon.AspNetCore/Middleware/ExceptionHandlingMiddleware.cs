using System;
using System.Threading.Tasks;
using Excepticon.AspNetCore.Model;
using Excepticon.AspNetCore.Options;
using Excepticon.AspNetCore.Services;
using Microsoft.AspNetCore.Http;

namespace Excepticon.AspNetCore.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExcepticonOptions _options;
        private readonly IExcepticonExceptionInstancesService _exceptionInstancesExcepticonExceptionInstancesService;

        public ExceptionHandlingMiddleware(RequestDelegate next, ExcepticonOptions options)
        {
            _next = next;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _exceptionInstancesExcepticonExceptionInstancesService = new ExcepticonExceptionInstancesService(options);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);

                if (!_options.ShouldSwallowExceptions)
                    throw;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var exceptionInstance = new ExceptionInstance(exception, context);

            await _exceptionInstancesExcepticonExceptionInstancesService.PostExceptionInstance(exceptionInstance, _options.ApiKey);
        }
    }
}
