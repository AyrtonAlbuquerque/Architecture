using System.Text;
using System.Text.Json;
using Architecture.Api.Abstractions.Services;
using EnumerableExtensions;

namespace Architecture.Api.Infrastructure.Services
{
    public class LogService(ILogger<LogService> logger) : ILogService
    {
        public async Task LogWarningAsync(HttpRequest request, Exception e = null)
        {
            await Log(request, e, 2);
        }

        public async Task LogErrorAsync(HttpRequest request, Exception e = null)
        {
            await Log(request, e, 1);
        }

        public async Task LogCriticalAsync(HttpRequest request, Exception e = null)
        {
            await Log(request, e, 0);
        }

        public async Task Log(string message, int type = 0)
        {
            try
            {
                if (type == 0)
                    logger.LogCritical(message);
                else if (type == 1)
                    logger.LogError(message);
                else
                    logger.LogWarning(message);
            }
            catch (Exception ex)
            {
                logger.LogCritical("Erro ao gravar log de erros: {Message}", (ex.InnerException ?? ex).Message);
            }
        }

        public async Task Log(string message, Exception e, int type = 0)
        {
            try
            {
                var sb = new StringBuilder();

                sb.AppendLine("----------------------------------- Message -----------------------------------");
                sb.AppendLine($"Message: {message}");
                sb.AppendLine("---------------------------------- Exception ----------------------------------");
                sb.AppendLine($"Exception: {e?.Message}");
                sb.AppendLine($"Inner Exception: {e?.InnerException?.Message}");
                sb.AppendLine($"Stack Trace: {e?.StackTrace}");
                sb.AppendLine($"Target Site: {e?.TargetSite}");
                sb.AppendLine($"Source: {e?.Source}");

                if (type == 0)
                    logger.LogCritical(sb.ToString());
                else if (type == 1)
                    logger.LogError(sb.ToString());
                else
                    logger.LogWarning(sb.ToString());
            }
            catch (Exception ex)
            {
                logger.LogCritical("Erro ao gravar log de erros: {Message}", (ex.InnerException ?? ex).Message);
            }
        }

        public async Task Log(HttpRequest request, Exception e = null, int type = 0)
        {
            try
            {
                request.Body.Position = 0;

                var sb = new StringBuilder();
                var headers = new StringBuilder();
                var body = request.Method != HttpMethods.Get ? JsonSerializer.Deserialize<object>(await new StreamReader(request.Body, Encoding.UTF8).ReadToEndAsync()) : string.Empty;

                request?.Headers.ForEach(x => headers.Append($"{x.Key}: {x.Value} "));

                sb.AppendLine("----------------------------------- Message -----------------------------------");
                sb.AppendLine($"Message: {e?.Message}");
                sb.AppendLine("----------------------------------- Request -----------------------------------");
                sb.AppendLine($"Request: {request?.Method} {request?.Path}");
                sb.AppendLine($"Headers: {headers}");
                sb.AppendLine($"Body: {JsonSerializer.Serialize(body)}");
                sb.AppendLine($"Query: {request?.QueryString}");
                sb.AppendLine("---------------------------------- Exception ----------------------------------");
                sb.AppendLine($"Exception: {e?.Message}");
                sb.AppendLine($"Inner Exception: {e?.InnerException?.Message}");
                sb.AppendLine($"Stack Trace: {e?.StackTrace}");
                sb.AppendLine($"Target Site: {e?.TargetSite}");
                sb.AppendLine($"Source: {e?.Source}");

                if (type == 0)
                    logger.LogCritical(sb.ToString());
                else if (type == 1)
                    logger.LogError(sb.ToString());
                else
                    logger.LogWarning(sb.ToString());
            }
            catch (Exception ex)
            {
                logger.LogCritical("Erro ao gravar log de erros: {Message}", (ex.InnerException ?? ex).Message);
            }
        }
    }
}