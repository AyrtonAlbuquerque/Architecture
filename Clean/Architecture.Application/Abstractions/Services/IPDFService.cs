namespace Architecture.Application.Abstractions.Services
{
    public interface IPDFService
    {
        Task<string> CreateUserPDF();
    }
}