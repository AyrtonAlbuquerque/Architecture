using Architecture.Application.Common;
using Architecture.Application.Abstractions;
using Architecture.Domain.Interfaces;
using FluentResults;

namespace Architecture.Application.Authentication.Login
{
    public class LoginHandler(IToken token, IUserRepository userRepository) : ICommandHandler<LoginCommand, LoginResponse>
    {
        public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(command.Email);

            if (user is null || !Hasher.Verify(command.Password, Hasher.Hash(user.Password)))
            {
                return Result.Fail("Incorrect email or password");
            }

            return Result.Ok(new LoginResponse("Bearer", token.Create(user)));
        }
    }
}