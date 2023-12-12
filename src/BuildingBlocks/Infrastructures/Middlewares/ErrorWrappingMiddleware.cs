using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddleware(ILogger logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var errorMsg = string.Empty;
            try
            {
                await _next.Invoke(context);
            }catch (Exception ex)
            {
                errorMsg = ex.Message;
                _logger.Error(errorMsg, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync("A Error was occured: " + exception.Message);
        }
    }
}
