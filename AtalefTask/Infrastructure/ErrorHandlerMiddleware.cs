using Newtonsoft.Json;

namespace AtalefTask.Infrastructure
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = string.Empty;

            if (exception is RestException)
            {
                RestException? restException = exception as RestException;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)restException?.Code!;
                result = JsonConvert.SerializeObject(new { StatusCode = restException.Code, restException.Message });
            }

            if (result == string.Empty)
            {
                result = JsonConvert.SerializeObject(new { Error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseRestExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
