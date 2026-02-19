using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Architecture.Api.Abstractions.Services;
using Architecture.Api.Common;

namespace Architecture.Api.Infrastructure.Services
{
    public class SMSService(Settings settings, HttpClient client) : ISMSService
    {
        private sealed class SMSResponse
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
        }

        public async Task<string> SendAsync(string phone, string text)
        {
            var request = JsonSerializer.Serialize(new
            {
                Carteira = settings.SMS.Carteira,
                Message = text,
                Phone = phone,
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
            var response = await client.PostAsync(settings.SMS.Endpoint, new StringContent(request, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) throw new Exception($"Erro na requisição de envio de SMS. Code: {response.StatusCode}, Content: {content}");

            return JsonSerializer.Deserialize<SMSResponse>(content)?.Id.ToString();
        }
    }
}