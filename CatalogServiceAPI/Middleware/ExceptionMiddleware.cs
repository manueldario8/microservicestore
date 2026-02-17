namespace CatalogServiceAPI.Middleware
{
    using CatalogServiceAPI.DomainExceptions;
    using CatalogServiceAPI.Responses;
    using System.Net;
    using System.Text.Json;

    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
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
            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BusinessException => HttpStatusCode.BadRequest,
                ConflictException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

            if ((int)statusCode >= 500)
            {
                _logger.LogError(exception, "Unhandled exception");
            }
            else
            {
                _logger.LogWarning(exception, "Handled exception");
            }

            var response = new ApiErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = exception.Message,
                Detail = statusCode == HttpStatusCode.InternalServerError
                            ? "An unexpected error occurred."
                            : null,
                CorrelationId = context.TraceIdentifier
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }

}
