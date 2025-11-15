using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AutoZone.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next; // الميدل وير اللي بعد دا
        private readonly ILogger<ErrorHandlerMiddleware> _logger; // لتسجيل الأخطاء

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next; // نحفظ الميدل وير اللي بعده
            _logger = logger; // نحفظ اللوجر
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // نعدي الريكوست
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // لو حصل خطأ نعالجه هنا
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode; // هنحدد نوع الكود حسب نوع الخطأ
            string message; // الرسالة اللي هتظهر للمستخدم

            // نحدد نوع الخطأ
            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized; // 401
                    message = "Unauthorized access."; // المستخدم مش مصرحله
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound; // 404
                    message = "Resource not found."; // العنصر مش موجود
                    break;

                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    message = exception.Message; // خطأ في البيانات اللي جاية
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError; // 500
                    message = "Internal Server Error."; // أي خطأ غير متوقع
                    break;
            }

            _logger.LogError(exception, "Exception caught by ErrorHandlerMiddleware."); // نحفظ الخطأ في اللوج

            // نجهز الريسبونس
            var response = new
            {
                statusCode = (int)statusCode,
                message,
#if DEBUG
                stackTrace = exception.StackTrace // نعرض الـ stack في الـ Development بس
#endif
            };

            var json = JsonSerializer.Serialize(response); // نحوله JSON

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(json); // نرجع الريسبونس
        }
    }
}
