using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Api.Handlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Server error",
                Status = httpContext.Response.StatusCode,
                Instance = httpContext.Request.Path
            });

            return true;
        }
    }
}