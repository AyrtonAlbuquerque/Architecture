namespace Architecture.Application.Abstractions.Services
{
    public interface ISMSService
    {
        Task<string> SendAsync(string phone, string text);
    }
}