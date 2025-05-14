using Architecture.Application.Abstractions;
using Architecture.Application.Common;
using Architecture.Domain.Entities;
using Architecture.Domain.Interfaces;
using FluentResults;
using Mapster;

namespace Architecture.Application.Authentication.Register
{
    public class Handler(IToken token, IUserRepository userRepository) : IHandler<Command, Response>
    {
        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var user = userRepository.Insert(command.Adapt<User>());

            if (await userRepository.ExistsAsync(command.Email))
            {
                return Result.Fail("User with this email already exists");
            }

            await userRepository.SaveAsync();

            return Result.Ok(new Response("Bearer", token.Create(user)));
        }
    }
}