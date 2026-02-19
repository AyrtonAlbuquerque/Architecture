using Architecture.Api.Extensions;
using Architecture.Application.Authentication.Register;
using MediatR;

namespace Architecture.Api.Endpoints.Authentication
{
    public class Register : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/register", async (Command command, ISender sender) =>
            {
                return (await sender.Send(command)).ToResult();
            })
            .WithTags("Auth")
            .WithValidation<Command>()
            .Produces<Response>();
        }
    }
}