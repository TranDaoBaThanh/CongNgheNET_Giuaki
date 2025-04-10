using System.Net;
using System.Text.Json;

namespace TodoApp.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");

            var statusCode = HttpStatusCode.InternalServerError; // 500 by default
            var errorMessage = "An error occurred processing your request.";

            // Customize response based on exception type
            if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = "You are not authorized to perform this action.";
            }
            else if (exception is ArgumentException || exception is FormatException || exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = "The request was invalid.";
            }

            // Set response code
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            // Create response object
            var responseObject = new
            {
                status = (int)statusCode,
                message = errorMessage,
                // Only include detailed error information in development
                detail = _env.IsDevelopment() ? exception.Message : null,
                stackTrace = _env.IsDevelopment() ? exception.StackTrace : null
            };

            var result = JsonSerializer.Serialize(responseObject);
            await context.Response.WriteAsync(result);

            // Check if we need to redirect to the error page for non-API requests
            if (!context.Request.Path.StartsWithSegments("/api") && !context.Response.HasStarted)
            {
                // Store the error details in session to display on the error page
                context.Session.SetString("ErrorMessage", errorMessage);
                context.Session.SetString("ErrorDetail", _env.IsDevelopment() ? exception.Message : "");
                
                // Redirect to the error page
                context.Response.Redirect($"/Home/Error?statusCode={(int)statusCode}");
            }
        }
    }
}
