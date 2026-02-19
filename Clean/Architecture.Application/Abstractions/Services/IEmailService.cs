namespace Architecture.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendAsync(string address, string subject, string body);
    }
}