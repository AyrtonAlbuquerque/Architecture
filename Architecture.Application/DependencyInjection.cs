using Architecture.Application.Services.Auth;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Architecture.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.Scan(scan => scan
                .FromAssemblyOf<AuthService>()
                .AddClasses(classes => classes.InNamespaces("Architecture.Application"))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }

}