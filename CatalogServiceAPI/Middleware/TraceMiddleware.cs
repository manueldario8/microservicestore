using System.Diagnostics;

namespace CatalogServiceAPI.Middleware
{
    public class TraceMiddleware(RequestDelegate next, ILogger<TraceMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<TraceMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = context.TraceIdentifier;
            var sw = Stopwatch.StartNew();

            _logger.LogInformation(
                "Request started | TraceId: {TraceId} | {Method} {Path}",
                traceId,
                context.Request.Method,
                context.Request.Path
            );

            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();

                _logger.LogInformation(
                    "Request finished | TraceId: {TraceId} | StatusCode: {StatusCode} | ElapsedMs: {ElapsedMs}",
                    traceId,
                    context.Response.StatusCode,
                    sw.ElapsedMilliseconds
                );
            }
        }
    }

}
