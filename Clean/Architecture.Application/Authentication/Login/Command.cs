using Architecture.Application.Abstractions;

namespace Architecture.Application.Authentication.Login
{
    public record Command(string Email, string Password) : ICommand<Response>;
}