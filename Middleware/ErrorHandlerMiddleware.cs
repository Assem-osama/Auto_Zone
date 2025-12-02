using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AutoZone.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (ex)
                {
                    case AutoZone.Exceptions.ValidationException e:
                        // custom application error
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        await response.WriteAsJsonAsync(new { message = e.Message });
                        break;
                    case AutoZone.Exceptions.NotFoundException e:
                        // not found error
                        response.StatusCode = StatusCodes.Status404NotFound;
                        await response.WriteAsJsonAsync(new { message = e.Message });
                        break;
                    default:
                        // unhandled error
                        _logger.LogError(ex, "Unhandled exception");
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        await response.WriteAsJsonAsync(new { message = "Internal server error" });
                        break;
                }
            }
        }
    }
}
