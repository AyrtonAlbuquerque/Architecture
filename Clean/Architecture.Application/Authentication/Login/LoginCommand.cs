using Architecture.Application.Common;
using Architecture.Application.Interfaces;

namespace Architecture.Application.Authentication.Login
{
    public record LoginCommand(string Email, string Password) : ICommand<Token>;
}