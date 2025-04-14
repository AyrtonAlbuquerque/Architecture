using Architecture.Api.Extensions;
using Architecture.Application.Common;
using Architecture.Application.Authentication.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Api.Endpoints.Authentication
{
    public class LoginEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async (LoginCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);

                return result.IsSuccess ? Results.Ok(result.Value) : result.Problem();
            })
            .WithTags("Auth")
            .WithValidation<LoginCommand>()
            .Produces<Token>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }
    }
}