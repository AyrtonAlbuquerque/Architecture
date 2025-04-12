using Architecture.Api.Extensions;
using Architecture.Application.Services.Auth.Contracts;
using Architecture.Application.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Api.Endpoints.Auth
{
    public class LoginEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async (Login login, IAuthService authService) =>
            {
                var result = await authService.Login(login);

                return result.IsSuccess ? Results.Ok(result.Value) : result.Problem();
            })
            .WithTags("Auth")
            .WithValidation<Login>()
            .Produces<Token>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }
    }
}