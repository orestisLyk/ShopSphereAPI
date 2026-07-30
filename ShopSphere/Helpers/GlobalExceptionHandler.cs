using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Exceptions;

namespace ShopSphere.Helpers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title, isExpected) = MapException(exception);

            if(isExpected)
            {
                logger.LogWarning(exception, "Handled expected exception: {ExceptionType} - {Message}", exception.GetType().Name, exception.Message);
            } else
            {
                logger.LogError(exception, "Unhandled exception: {ExceptionType} - {Message}", exception.GetType().Name, exception.Message);
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = isExpected ? exception.Message : "An unexpected error occurred.",
                Instance = httpContext.Request.Path,
                Type = $"https://httpstatuses.com/{statusCode}"
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static (int StatusCode, string Title, bool isExpected) MapException(Exception ex) => ex switch
        {
            EntityNotFoundException => (StatusCodes.Status404NotFound, "Entity Not Found", true),
            EntityAlreadyExistsException => (StatusCodes.Status409Conflict, "Entity Already Exists", true),
            ConfigurationException => (StatusCodes.Status500InternalServerError, "Configuration Error", false),
            ImageUploadException => (StatusCodes.Status500InternalServerError, $"{ex.Message}", true),
            InsufficientStockException => (StatusCodes.Status409Conflict, "Insufficient Stock", true),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized Access", true),
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request", true),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid Operation", true),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.", false)
        };
        
    }
}
