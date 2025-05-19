using System.Text.Json;

namespace BookLibraryAPI.Middleware
{
    /// <summary>
    /// RequestLoggingMiddleware it middleware
    /// </summary>
    public class RequestLoggingMiddleware
    {
        #region Private Variable
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
        }

        #endregion

        #region Public Methord
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation($"HTTP {context.Request.Method} - {context.Request.Path}");

                // Continue processing the request
                await _next(context);

                _logger.LogInformation("Response Status: {status}", context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Set response details
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    Message = "An internal server error occurred.",
                    Details = ex.Message // Optionally: hide this in production
                };

                // Serialize and write to response
                var json = JsonSerializer.Serialize(result);
                await context.Response.WriteAsync(json);
            }
        }

        #endregion

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
