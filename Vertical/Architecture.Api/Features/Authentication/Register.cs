using Architecture.Api.Abstractions;
using Architecture.Api.Common;
using Architecture.Api.Domain.Entities;
using Architecture.Api.Domain.Interfaces;
using Architecture.Api.Extensions;
using FluentResults;
using FluentValidation;
using Mapster;
using MediatR;

namespace Architecture.Api.Features.Authentication
{
    public static class Register
    {
        public sealed record Command(string Email, string Password) : ICommand<Response>;
        public sealed record Response(string Type, string Value);

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

        public sealed class Mapping : IMapping
        {
            public void AddMapping()
            {
                TypeAdapterConfig<Command, User>
                    .NewConfig()
                    .Map(dest => dest.Email, source => source.Email)
                    .Map(dest => dest.Password, source => Hasher.Hash(source.Password));
            }
        }

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("/auth/register", async (Command command, ISender sender) =>
                {
                    var result = await sender.Send(command);

                    return result.IsSuccess ? Results.Ok(result.Value) : result.Problem();
                })
                .WithTags("Auth")
                .WithValidation<Command>()
                .Produces<Response>();
            }
        }

        public sealed class Handler(IToken token, IUserRepository userRepository) : ICommandHandler<Command, Response>
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
}