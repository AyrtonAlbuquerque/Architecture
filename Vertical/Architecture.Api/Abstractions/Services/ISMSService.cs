namespace Architecture.Api.Abstractions.Services
{
    public interface ISMSService
    {
        Task<string> SendAsync(string phone, string text);
    }
}