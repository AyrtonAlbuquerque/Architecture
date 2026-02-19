using Microsoft.AspNetCore.Http;

namespace Architecture.Application.Abstractions.Services
{
    public interface ILogService
    {
        Task LogWarningAsync(HttpRequest request, Exception e = null);
        Task LogErrorAsync(HttpRequest request, Exception e = null);
        Task LogCriticalAsync(HttpRequest request, Exception e = null);
        Task Log(string message, int type = 0);
        Task Log(string message, Exception e, int type = 0);
        Task Log(HttpRequest request, Exception e = null, int type = 0);
    }
}