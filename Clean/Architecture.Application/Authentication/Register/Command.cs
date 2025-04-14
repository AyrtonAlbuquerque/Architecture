using Architecture.Application.Abstractions;

namespace Architecture.Application.Authentication.Register
{
    public record Command(string Email, string Password) : ICommand<Response>;
}