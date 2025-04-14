using Architecture.Application.Common;
using Architecture.Application.Interfaces;
using Architecture.Domain.Interfaces;
using FluentResults;

namespace Architecture.Application.Authentication.Login
{
    public class LoginHandler(IUserRepository userRepository) : ICommandHandler<LoginCommand, Token>
    {
        public async Task<Result<Token>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(command.Email);

            if (user is null || !Hasher.Verify(command.Password, user.Password))
            {
                return Result.Fail("Incorrect email or password");
            }

            return Result.Ok(new Token(user));
        }
    }
}