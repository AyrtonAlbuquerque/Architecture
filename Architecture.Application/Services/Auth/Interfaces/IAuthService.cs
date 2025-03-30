using Architecture.Application.Services.Auth.Contracts;
using FluentResults;

namespace Architecture.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<Result<Token>> Login(Login login);
    }

}