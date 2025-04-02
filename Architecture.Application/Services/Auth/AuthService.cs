using Architecture.Application.Options;
using Architecture.Application.Services.Auth.Contracts;
using Architecture.Application.Services.Auth.Interfaces;
using Architecture.Domain.Interfaces;
using FluentResults;

namespace Architecture.Application.Services.Auth
{
    public class AuthService(IUserRepository userRepository, Settings settings) : IAuthService
    {
        public async Task<Result<Token>> Login(Login login)
        {
            try
            {
                await userRepository.SelectAsync();

                return Result.Ok(new Token(login, settings));
            }
            catch (Exception e)
            {
                return Result.Fail(new Error("Login Error").CausedBy((e.InnerException ?? e).Message));
            }
        }
    }
}