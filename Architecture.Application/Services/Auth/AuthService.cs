using Architecture.Application.Services.Auth.Contracts;
using Architecture.Application.Services.Auth.Interfaces;
using Architecture.Domain.Interfaces;
using FluentResults;
using Microsoft.Extensions.Configuration;

namespace Architecture.Application.Services.Auth
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
    {
        public async Task<Result<Token>> Login(Login login)
        {
            var secret = configuration["AppSettings:Secret"];
            var session = double.Parse(configuration["AppSettings:SessionTime"]);

            try
            {
                await userRepository.SelectAsync();

                return Result.Ok(new Token(login, secret, session));
            }
            catch (Exception e)
            {
                return Result.Fail(new Error("Login Error").CausedBy((e.InnerException ?? e).Message));
            }
        }
    }
}