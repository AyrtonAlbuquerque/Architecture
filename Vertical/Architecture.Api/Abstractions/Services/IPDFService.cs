namespace Architecture.Api.Abstractions.Services
{
    public interface IPDFService
    {
        Task<string> CreateUserPDF();
    }
}