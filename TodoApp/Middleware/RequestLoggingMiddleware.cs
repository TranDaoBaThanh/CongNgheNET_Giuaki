using System.Diagnostics;

namespace TodoApp.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;
            var requestId = Activity.Current?.Id ?? context.TraceIdentifier;

            try
            {
                // Log the incoming request
                _logger.LogInformation(
                    "Request started: {RequestMethod} {RequestPath} - RequestId: {RequestId}",
                    requestMethod, requestPath, requestId);

                // Call the next middleware in the pipeline
                await _next(context);

                // Log the response
                stopwatch.Stop();
                _logger.LogInformation(
                    "Request completed: {RequestMethod} {RequestPath} - StatusCode: {StatusCode} - Elapsed: {ElapsedMs}ms - RequestId: {RequestId}",
                    requestMethod, requestPath, context.Response.StatusCode, stopwatch.ElapsedMilliseconds, requestId);
            }
            catch (Exception ex)
            {
                // Log the exception
                stopwatch.Stop();
                _logger.LogError(
                    ex,
                    "Request failed: {RequestMethod} {RequestPath} - Elapsed: {ElapsedMs}ms - RequestId: {RequestId}",
                    requestMethod, requestPath, stopwatch.ElapsedMilliseconds, requestId);

                // Re-throw the exception to be handled by the error handler middleware
                throw;
            }
        }
    }
}
