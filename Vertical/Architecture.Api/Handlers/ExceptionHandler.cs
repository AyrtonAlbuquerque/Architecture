using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Api.Handlers
{
    public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Server error",
                Status = httpContext.Response.StatusCode,
                Instance = httpContext.Request.Path
            }, cancellationToken);

            return true;
        }
    }
}