using Architecture.Application.Common;
using Architecture.Application.Abstractions;
using Architecture.Domain.Interfaces;
using FluentResults;

namespace Architecture.Application.Authentication.Login
{
    public class Handler(IToken token, IUserRepository userRepository) : ICommandHandler<Command, Response>
    {
        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(command.Email);

            if (user is null || !Hasher.Verify(command.Password, user.Password))
            {
                return Result.Fail("Incorrect email or password");
            }

            return Result.Ok(new Response("Bearer", token.Create(user)));
        }
    }
}