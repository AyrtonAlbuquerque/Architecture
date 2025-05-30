using Architecture.Api.Extensions;
using Architecture.Application.Authentication.Login;
using MediatR;

namespace Architecture.Api.Endpoints.Authentication
{
    public class Login : IEndpoint
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
            .Produces<Response>();
        }
    }
}