using Architecture.Api.Extensions;
using Architecture.Application.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Api.Endpoints.Authentication
{
    public class Register : IEndpoint
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
}