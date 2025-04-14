using Architecture.Application.Abstractions;

namespace Architecture.Application.Authentication.Login
{
    public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
}