using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Architecture.Api.Domain.Interfaces;
using Architecture.Api.Extensions;
using Architecture.Api.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Architecture.Api.Features.Authentication
{
    public static class Login
    {
        public record Request(string Username, string Password);
        public record Response(string Type, double? Expires, string Value);

        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Username)
                    .NotEmpty()
                    .WithMessage("Username is required");
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("Password is required");
            }
        }

        public sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapPost("/auth/login", Handler)
                    .WithTags("Auth")
                    .WithValidation<Request>()
                    .Produces<Response>()
                    .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
                    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
            }
        }

        public static async Task<IResult> Handler(Request request, Settings settings, IUserRepository userRepository)
        {
            await userRepository.SelectAsync();

            return Results.Ok(new Response
            (
                "Bearer",
                TimeSpan.FromHours(settings.Jwt.Expiration).TotalMilliseconds,
                new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                    issuer: settings.Jwt.Issuer,
                    audience: settings.Jwt.Audience,
                    claims: new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                    },
                    expires: DateTime.Now.AddHours(settings.Jwt.Expiration),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Jwt.Secret)), SecurityAlgorithms.HmacSha256Signature)
                ))
            ));
        }
    }
}