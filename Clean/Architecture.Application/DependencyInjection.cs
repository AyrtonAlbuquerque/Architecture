using Architecture.Application.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Architecture.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            services.AddOptions<Settings>()
                .BindConfiguration(Settings.Section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddSingleton(x => x.GetRequiredService<IOptions<Settings>>().Value);
            services.AddSingleton<IToken, Token>();

            return services;
        }
    }
}