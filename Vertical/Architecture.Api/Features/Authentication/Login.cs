using Architecture.Api.Abstractions;
using Architecture.Api.Common;
using Architecture.Api.Domain.Interfaces;
using Architecture.Api.Extensions;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Api.Features.Authentication
{
    public static class Login
    {
        public record Command(string Email, string Password) : ICommand<Token>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("Email is required");
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("Password is required");
            }
        }

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("/auth/login", async (Command command, ISender sender) =>
                {
                    var result = await sender.Send(command);

                    return result.IsSuccess ? Results.Ok(result.Value) : result.Problem();
                })
                .WithTags("Auth")
                .WithValidation<Command>()
                .Produces<Token>()
                .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
                .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
            }
        }

        public sealed class Handler(IUserRepository userRepository) : ICommandHandler<Command, Token>
        {
            public async Task<Result<Token>> Handle(Command command, CancellationToken cancellationToken)
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
}